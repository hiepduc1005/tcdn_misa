<script setup>
import { reactive, ref, watch } from 'vue';
import BaseButton from '../../components/button/BaseButton.vue';
import ShiftBody from './ShiftBody.vue';
import ShiftFormModal from './ShiftFormModal.vue';
import { COLUMN_TYPE, NUMBER_DATE_FILTER_OPERATORS, SHIFT_MODAL_TYPE, TEXT_FILTER_OPERATORS } from '../../constants/common';
import ShiftAPI from '../../apis/components/shift/ShiftAPI';
import { formatDate } from '../../utils/formatDateFns';
import { camelToPascalCase, formatTimeToHHMM, roundNumber } from '../../utils/formatFns';
import BaseToolTip from '../../components/tooltip/BaseToolTip.vue';
import BaseModal from '../../components/modal/BaseModal.vue';
import { ElMessage } from 'element-plus';

/**
 * Trạng thái paging dùng để gửi lên backend khi gọi API paging
 * Bao gồm: filter, sort, pageSize, pageIndex
 *
 * - filterItems   : Filter theo từng cột
 * - customFilters : Search
 * - sortItems     : Danh sách sắp xếp
 * - pageSize      : Số bản ghi trên 1 trang
 * - pageIndex     : Trang hiện tại (bắt đầu từ 1)
 *
 * Created By: hiepnd - 12/2025
 */
const pagingState = reactive({
  filterItems: [],
  customFilters: [],
  sortItems: [],
  pageSize: 20,
  pageIndex: 1
});

/**
 * Kết quả trả về sau khi gọi API paging
 *
 * - data         : Danh sách bản ghi của trang hiện tại
 * - totalRecord : Tổng số bản ghi
 * - totalPage   : Tổng số trang
 *
 * Created By: hiepnd - 12/2025
 */
const pagingResult = reactive({
  data: [],
  totalRecord: 0,
  totalPage: 0,
});

// Biến loading
const isLoading = ref(false);

/**
 * Hàm tải dữ liệu ca làm việc (Shift) có phân trang từ server.
 * - Gọi API paging với trạng thái phân trang hiện tại
 * - Cập nhật danh sách dữ liệu, tổng số bản ghi và tổng số trang
 * - Quản lý trạng thái loading trong quá trình gọi API
 *
 * Created By: hiepnd - 12/2025
 */
const loadDataPagingShift = async () => {
    // Bật trạng thái loading khi bắt đầu gọi API
    isLoading.value = true;

    try {

        // Gọi API lấy dữ liệu phân trang
        const response = await ShiftAPI.paging(pagingState);

        // Kiểm tra lỗi trả về từ backend
        if(response.errors && response.statusCode >= 400){
            console.error("loadDataPagingShift :" , response.errors);
            return;
        }

        pagingResult.data = response?.data.data?.dataPaging;
        pagingResult.totalPage = response?.data.data?.totalPages;
        pagingResult.totalRecord = response?.data.data?.totalRecords;
    } catch (error) {

        // Bắt và log lỗi khi gọi API thất bại
        console.error("Error loading data:", error);
    } finally {

        // Tắt trạng thái loading sau khi xử lý xong
        isLoading.value = false;
    }
}

/**
 * Hàm ngừng sử dụng một hoặc nhiều ca làm việc.
 * - Nhận vào một Id hoặc danh sách Id ca làm việc
 * - Chuẩn hóa dữ liệu đầu vào thành mảng trước khi gọi API
 *
 * Created By: hiepnd - 12/2025
 */
const handleInactiveAll = async (shiftIds) => {
    // Nếu chỉ truyền vào 1 Id thì chuyển thành mảng
    if (!Array.isArray(shiftIds)) {
        shiftIds = [shiftIds];
    }

    // Gọi API ngừng sử dụng ca làm việc
    await ShiftAPI.inactiveAll(shiftIds);
}

/**
 * Hàm kích hoạt (sử dụng) một hoặc nhiều ca làm việc.
 * - Nhận vào một Id hoặc danh sách Id ca làm việc
 * - Chuẩn hóa dữ liệu đầu vào thành mảng trước khi gọi API
 *
 * Created By: hiepnd - 12/2025
 */
const handleActiveAll = async (shiftIds) => {
    // Nếu chỉ truyền vào 1 Id thì chuyển thành mảng
    if (!Array.isArray(shiftIds)) {
        shiftIds = [shiftIds];
    }

    // Gọi API kích hoạt ca làm việc
    await ShiftAPI.activeAll(shiftIds);
}


/**
 * Theo dõi sự thay đổi của trạng thái phân trang, sắp xếp và lọc dữ liệu.
 * - Khi pageSize, pageIndex, sortItems, customFilters hoặc filterItems thay đổi
 *   thì tự động gọi lại API để load dữ liệu mới
 * - deep: true     → theo dõi thay đổi sâu trong object/array
 * - immediate: true → gọi API ngay lần đầu khi component được mount
 *
 * Created By: hiepnd - 12/2025
 */
watch(
    () => [
    pagingState.pageSize,
    pagingState.pageIndex,
    pagingState.sortItems,
    pagingState.customFilters,
    pagingState.filterItems
  ],
    () => {
        loadDataPagingShift();
    },
    {deep: true, immediate: true}
)


/**
 * Cấu hình danh sách cột hiển thị cho bảng Ca làm việc (Shift).
 * - field      : Tên field tương ứng với dữ liệu backend
 * - label      : Tiêu đề hiển thị trên UI
 * - type       : Kiểu dữ liệu của cột (text, number, date, select…)
 * - filterable : Cho phép lọc dữ liệu theo cột hay không
 * - options    : Danh sách toán tử lọc
 * - right      : Căn phải nội dung cột (thường dùng cho số)
 *
 * Created By: hiepnd - 12/2025
 */
const columns = [
    { 
        field: "shiftCode", 
        label: "Mã ca", 
        filterable: true,
        type: COLUMN_TYPE.TEXT,
        options:  TEXT_FILTER_OPERATORS,
        right: false, 
    },
    { 
        field: "shiftName", 
        label: "Tên ca", 
        filterable: true,
        options:  TEXT_FILTER_OPERATORS,
        type: COLUMN_TYPE.TEXT,
        right: false, 
    },
   
    { 
        field: "beginShiftTime", 
        label: "Giờ vào ca", 
        filterable: false, 
        type: COLUMN_TYPE.NUMBER,
        right: false, 
    },
    { 
        field: "endShiftTime", 
        label: "Giờ hết ca", 
        filterable: false, 
        type: COLUMN_TYPE.NUMBER,
        right: false, 
    },
    { 
        field: "beginBreakTime", 
        label: "Bắt đầu nghỉ giữa ca", 
        filterable: false, 
        type: COLUMN_TYPE.NUMBER,
        right: false, 
    },
    { 
        field: "endBreakTime", 
        label: "Kết thúc nghỉ giữa ca", 
        filterable: false, 
        type: COLUMN_TYPE.NUMBER,
        right: false, 
    },
    { 
        field: "workingTime", 
        label: "Thời gian làm việc (giờ)", 
        type: COLUMN_TYPE.NUMBER,
        options:  NUMBER_DATE_FILTER_OPERATORS,
        filterable: true, 
        right: true, 
    },
    { 
        field: "breakingTime", 
        label: "Thời gian nghỉ giữa ca (giờ)",
        type: COLUMN_TYPE.NUMBER,
        options:  NUMBER_DATE_FILTER_OPERATORS,
        filterable: true, 
        right: true, 
    },
    { 
        field: "inactive", 
        label: "Trạng thái",
        type: COLUMN_TYPE.SELECT,
        options: [
            { label: 'Đang sử dụng', value: false },
            { label: 'Ngừng sử dụng', value: true }
        ],
        filterable: true, 
        right: false, 
    },
    { field: "createdBy", label: "Người tạo", filterable: true, type: COLUMN_TYPE.TEXT,  options:  TEXT_FILTER_OPERATORS, right: false },
    { field: "createdDate", label: "Ngày tạo", filterable: true, type: COLUMN_TYPE.DATE,   options:  NUMBER_DATE_FILTER_OPERATORS, right: false },
    { field: "modifiedBy", label: "Người sửa", filterable: true, type: COLUMN_TYPE.TEXT, options:  TEXT_FILTER_OPERATORS, right: false},
    { field: "modifiedDate", label: "Ngày sửa", filterable: true, type: COLUMN_TYPE.DATE, options:  NUMBER_DATE_FILTER_OPERATORS, right: false},
];

/**
 * Trạng thái hiển thị form thêm/sửa ca làm việc.
 * Created By: hiepnd - 12/2025
 */
const showFormModal = ref(false);

/**
 * Ca làm việc đang được chọn (dùng cho xem / sửa).
 * Created By: hiepnd - 12/2025
 */
const selectedShift = ref({});

/**
 * Loại modal (Tạo mới / Chỉnh sửa).
 * Created By: hiepnd - 12/2025
 */
const modalType = ref(SHIFT_MODAL_TYPE.CREATE);

/**
 * Hiển thị form modal thêm mới / chỉnh sửa ca làm việc.
 * - type : Loại modal (CREATE | UPDATE)
 * - data : Dữ liệu ca làm việc được truyền vào form
 *
 * Created By: hiepnd - 12/2025
 */
const handleShowFormModal = (type = SHIFT_MODAL_TYPE.CREATE, data = {}) => {
    modalType.value = type;
    selectedShift.value = data;
    showFormModal.value = true;
}

/**
 * Đóng form modal ca làm việc.
 *
 * Created By: hiepnd - 12/2025
 */
const handleCloseModal = () => {
    showFormModal.value = false;
}

/**
 * Xử lý sự kiện chỉnh sửa ca làm việc.
 * - Mở form modal ở chế độ UPDATE
 *
 * Created By: hiepnd - 12/2025
 */
const handleEdit = (row) => {
    handleShowFormModal(SHIFT_MODAL_TYPE.UPDATE, row);
}

/**
 * Xử lý nhân bản (duplicate) ca làm việc.
 * - Lấy dữ liệu mới nhất từ server theo Id
 * - Xóa các trường định danh để tạo mới
 * - Mở form modal ở chế độ CREATE với dữ liệu đã nhân bản
 *
 * Created By: hiepnd - 12/2025
 */
const handleDuplicate = async (row) => {
    let duplicatedData = { ...row };

    const response = await ShiftAPI.getById(row.shiftId);

    if(response.errors && response.statusCode >= 400){
        console.error("handleDuplicate :" , response.errors);
        return;
    }

    duplicatedData = {...response?.data.data};
      
    // Xóa định danh để tạo mới
    delete duplicatedData.shiftCode;
    delete duplicatedData.shiftId;
    
    handleShowFormModal(SHIFT_MODAL_TYPE.CREATE, duplicatedData);
}

/**
 * Cập nhật trạng thái sử dụng của một hoặc nhiều ca làm việc.
 * - Cập nhật trước trên frontend để phản hồi nhanh UI
 * - Sau đó gọi API để cập nhật dữ liệu backend
 *
 * Created By: hiepnd - 12/2025
 */
const handleUpdateStatus = async ({ ids, status }) => {
    // Cập nhật trạng thái tạm thời trên frontend
    pagingResult.data.forEach(item => {
        if (ids.includes(item.shiftId)) {

            // Cập nhập ở frontend
            item.inactive = status;
            item.modifiedDate = new Date().toISOString();
            item.modifiedBy = "Admin";
        }
    });


    try {                  
        // Gọi API cập nhật trạng thái                                                                                    
        if (status === false) {                                                                                
            await handleActiveAll(ids);                                                                     
        } else {                                                                                              
            await handleInactiveAll(ids);                                                                   
        }                                                                                                     
    } catch (error) {                                                                                          
        console.error("Lỗi cập nhật trạng thái:", error);                                                      
    }
}

/**
 * Trạng thái hiển thị modal xác nhận xóa ca làm việc.
 * Created By: hiepnd - 12/2025
 */
const showDeleteModal = ref(false);

/**
 * Danh sách Id ca làm việc cần xóa.
 * Created By: hiepnd - 12/2025
 */
const idsToDelete = ref([]);

/**
 * Nội dung thông báo hiển thị trong modal xác nhận xóa.
 * Created By: hiepnd - 12/2025
 */
const deleteMessage = ref('');

/**
 * Xử lý yêu cầu xóa ca làm việc.
 * - Nhận vào danh sách Id ca làm việc
 * - Hiển thị thông báo xác nhận phù hợp (1 bản ghi hoặc nhiều bản ghi)
 *
 * Created By: hiepnd - 12/2025
 */
const handleRequestDelete = (shiftIds) => {
    idsToDelete.value = shiftIds.shiftIds;
    if (shiftIds.shiftIds.length === 1) {
        // Tìm mã ca làm việc cần xóa để hiển thị trong thông báo
        const item = pagingResult.data.find(i => i.shiftId === shiftIds.shiftIds[0]);
        const code = item ? item.shiftCode : '';
        deleteMessage.value = `Ca làm việc <b>${code}</b> sau khi bị xóa sẽ không thể khôi phục. Bạn có muốn tiếp tục xóa không?`;
    } else {
        deleteMessage.value = `Các ca làm việc đã chọn sau khi bị xóa sẽ không thể khôi phục. Bạn có muốn tiếp tục xóa không?`;
    }
    showDeleteModal.value = true;
};

/**
 * Xác nhận xóa ca làm việc.
 * - Gọi API xóa ca làm việc
 * - Hiển thị thông báo thành công
 * - Reload lại dữ liệu phân trang
 *
 * Created By: hiepnd - 12/2025
 */
const confirmDelete = async () => {
    try {
        await ShiftAPI.deleteShifts(idsToDelete.value);
        showSuccessToast("Xóa Ca làm việc thành công")
        // Refresh lại dữ liệu sau khi xóa thành công
        await loadDataPagingShift();
        showDeleteModal.value = false;
        idsToDelete.value = [];
    } catch (error) {
        //Log lỗi 
        console.error("Lỗi khi xóa:", error);
    }
};

/**
 * Hủy thao tác xóa ca làm việc.
 * - Đóng modal xác nhận
 * - Reset danh sách Id cần xóa
 *
 * Created By: hiepnd - 12/2025
 */
const cancelDelete = () => {
    showDeleteModal.value = false;
    idsToDelete.value = [];
};

/**
 * Hiển thị thông báo toast thành công.
 *
 * Created By: hiepnd - 12/2025
 */
const showSuccessToast = (message) => {
  ElMessage({
    message: message ,
    type: "success",
    customClass: "misa-toast",
    duration: 3000,
    showClose: true,
    offset: 32
    });
}

/**
 * Xử lý sau khi thêm mới hoặc chỉnh sửa ca làm việc thành công.
 * - Reload lại dữ liệu
 * - Hiển thị thông báo phù hợp theo loại modal
 *
 * Created By: hiepnd - 12/2025
 */
const handleSuccessCreate = async () => {
    await loadDataPagingShift();
    if(modalType.value === SHIFT_MODAL_TYPE.CREATE){
        showSuccessToast("Thêm ca làm việc thành công");
    }else showSuccessToast("Sửa ca làm việc thành công");
}

/**
 * Xử lý tìm kiếm ca làm việc theo từ khóa.
 * - Tìm kiếm theo mã ca, tên ca và mô tả
 *
 * Created By: hiepnd - 12/2025
 */
const handleSearch = (val) => {
    const filterCode = {
        'column' : "ShiftCode",
        'value'  : val,
        'operator' : 'contains'
    }
    const filterName = {
        'column' : "ShiftName",
        'value'  : val,
        'operator' : 'contains'
    }
    const filterDescription = {
        'column' : "Description",
        'value'  : val,
        'operator' : 'contains'
    }

    pagingState.customFilters = [filterCode,filterName,filterDescription];
}

/**
 * Xử lý thay đổi số bản ghi trên một trang.
 * - Reset về trang đầu tiên khi thay đổi pageSize
 *
 * Created By: hiepnd - 12/2025
 */
const handleChangePageSize = (val) => {
    pagingState.pageSize = val

    //Reset về trang đầu tiên khi thay đổi pageSize
    pagingState.pageIndex = 1
}

/**
 * Xử lý thay đổi trang hiện tại.
 *
 * Created By: hiepnd - 12/2025
 */
const handleChangePageIndex = (val) => {
    pagingState.pageIndex = val
}

/**
 * Xử lý thay đổi bộ lọc từ bảng dữ liệu.
 * - Chuyển field từ camelCase sang PascalCase để gửi lên backend
 *
 * Created By: hiepnd - 12/2025
 */
const handleFilterChange = (filters) => {

    pagingState.filterItems = filters.map(filter => {
        return {
            column: camelToPascalCase(filter.field),
            value: filter.value + "",
            operator: filter.operator + ""
        }
    });
}

/**
 * Xử lý khi người dùng thay đổi sắp xếp (sort) trên bảng dữ liệu.
 * - Nhận danh sách sort từ UI
 * - Chuyển fieldName từ camelCase (frontend)
 *   sang PascalCase để backend nhận đúng
 * - Gán lại vào pagingState.sortItems để dùng cho paging
 *
 * Created By: hiepnd - 12/2025
 */
const handleSortChange = (sorts) => {
    pagingState.sortItems = sorts.map(sort => {
        return {

            // Tên field gửi lên backend (chuẩn PascalCase)
            fieldName: camelToPascalCase(sort.fieldName),

            // ASC hoặc DESC
            direction: sort.direction
        }
    })
}

/**
 * Xử lý xuất danh sách ca làm việc ra file Excel.
 * - Gửi toàn bộ trạng thái paging (filter, sort, pageSize, pageIndex)
 *   lên backend để export đúng dữ liệu đang xem
 * - Nhận file Excel dạng Blob từ API
 * - Tạo link tạm thời và tự động trigger download
 *
 * Created By: hiepnd - 12/2025
 */
const handleExportExcel = async () => {
    try {
        //Gọi API export Excel.
        const response = await ShiftAPI.exportExcel(pagingState);

        //Tạo URL tạm thời từ Blob để trình duyệt có thể tải file
        const url = window.URL.createObjectURL(new Blob([response.data]));

        //Tạo thẻ <a> ảo để trigger download
        const link = document.createElement('a');
        link.href = url;

        //Lấy tên file từ header `content-disposition`
        const contentDisposition = response.headers['content-disposition'];
        let fileName = 'Danh_sach_ca.xlsx'; // Tên mặc định nếu không lấy được
        
        if (contentDisposition) {
            //Regex để lấy filename trong header
            const match = contentDisposition.match(/filename="?([^"]+)"?/);
            if (match && match[1]) {
                fileName = decodeURIComponent(match[1]); // Decode để hiển thị tiếng Việt đúng
            }
        }

        //Gán tên file cho thuộc tính download
        link.setAttribute('download', fileName);

        //Append link vào DOM để click được
        document.body.appendChild(link);

        //Trigger download file
        link.click();
        
        //Dọn dẹp DOM và giải phóng bộ nhớ
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    } catch (error) {
        console.error("Lỗi xuất khẩu:", error);
        // toast.error("Có lỗi xảy ra khi xuất khẩu.");
    } finally {
        // Tắt loading
        // isLoading.value = false;
    }
}

</script>

<template>
    <div class="shifview flex flex-column content-center">
        <div class="shifview-header flex flex-row align-center between">
            <div class="shifview-header-title">Ca làm việc</div>
            <div class="shiftview-header-addbtn">
                <BaseButton
                    :type="'solid-brand'"
                    @click="handleShowFormModal(SHIFT_MODAL_TYPE.CREATE)"
                >
                    <template #content>
                        <div class="icon16 icon-add-white"></div>
                        <span class="nowrap">Thêm</span>
                    </template>
                </BaseButton>
            </div>
        </div>
        <div class="shifview-body">
            <ShiftBody
                :datas="pagingResult.data"
                :columns="columns"
                :pageIndex="pagingState.pageIndex"
                :pageSize="pagingState.pageSize"
                :totalPage="pagingResult.totalPage"
                :totalRecord="pagingResult.totalRecord"
                :isLoading="isLoading"
                @update-status="handleUpdateStatus"
                @request-delete="handleRequestDelete"
                @edit="handleEdit"
                @duplicate="handleDuplicate"
                @search="handleSearch"
                @refresh="loadDataPagingShift"
                @page-size="handleChangePageSize"
                @page-index="handleChangePageIndex"
                @filter="handleFilterChange"
                @sort="handleSortChange"
                @export="handleExportExcel"
            >
                <template #body-workingTime="{ value }">
                    <BaseToolTip :placement="'bottom-start'" :showArror="false">
                        <template #title>
                            <span :style="value < 0 ? 'color: red;' : ''">
                                {{ value < 0 ? `(${Math.abs(roundNumber(value))})` : roundNumber(value) }}
                            </span>
                        </template>
                        <template #content>
                            <span :style="value < 0 ? 'color: red;' : ''">
                                {{ value < 0 ? `(${Math.abs(roundNumber(value))})` : roundNumber(value) }}
                            </span>
                        </template>
                    </BaseToolTip>
                </template>
                <template #body-breakingTime="{ value }">
                    <BaseToolTip :placement="'bottom-start'" :showArror="false">
                        <template #title>
                            <span :style="value < 0 ? 'color: red;' : ''">
                                {{ value < 0 ? `(${Math.abs(roundNumber(value))})` : roundNumber(value) }}
                            </span>
                        </template>
                        <template #content>
                             <span :style="value < 0 ? 'color: red;' : ''">
                                {{ value < 0 ? `(${Math.abs(roundNumber(value))})` : roundNumber(value) }}
                            </span>
                        </template>
                    </BaseToolTip>
                </template>
                <template #body-inactive="{ value }">
                    <div class="flex flex-row align-center" :class="value ? 'inactive' : 'active'">
                        <span>{{ value ? 'Ngừng sử dụng' : 'Đang sử dụng' }}</span>
                    </div>
                </template>
                <template #body-beginShiftTime="{ value }">
                    <BaseToolTip :placement="'bottom-start'" :showArror="false">
                        <template #title><span>{{ formatTimeToHHMM(value) }}</span></template>
                        <template #content><span>{{ formatTimeToHHMM(value) }}</span></template>
                    </BaseToolTip>
                </template>
                <template #body-endShiftTime="{ value }">
                    <BaseToolTip :placement="'bottom-start'" :showArror="false">
                        <template #title><span>{{ formatTimeToHHMM(value) }}</span></template>
                        <template #content><span>{{ formatTimeToHHMM(value) }}</span></template>
                    </BaseToolTip>
                </template>
                <template #body-beginBreakTime="{ value }">
                    <BaseToolTip :placement="'bottom-start'" :showArror="false">
                        <template #title><span>{{ formatTimeToHHMM(value) }}</span></template>
                        <template #content><span>{{ formatTimeToHHMM(value) }}</span></template>
                    </BaseToolTip>
                </template>
                <template #body-endBreakTime="{ value }">
                    <BaseToolTip :placement="'bottom-start'" :showArror="false">
                        <template #title><span>{{ formatTimeToHHMM(value) }}</span></template>
                        <template #content><span>{{ formatTimeToHHMM(value) }}</span></template>
                    </BaseToolTip>
                </template>
                <template #body-createdDate="{ value }">
                    <BaseToolTip
                        :placement="'bottom-start'"
                        :showArror="false"
                    >
                        <template #title>
                            <span>{{ formatDate(value) }}</span>
                        </template>

                        <template #content>
                            <span>{{ formatDate(value) }}</span>
                        </template>
                    </BaseToolTip>
                </template>
                <template #body-modifiedDate="{ value }">
                    <BaseToolTip
                        :placement="'bottom-start'"
                        :showArror="false"
                    >
                        <template #title>
                            <span>{{ formatDate(value) }}</span>
                        </template>

                        <template #content>
                            <span>{{ formatDate(value) }}</span>
                        </template>
                    </BaseToolTip>
                </template>

                
            </ShiftBody>
            <ShiftFormModal 
                :modalType="modalType"
                :data="selectedShift"
                :showModal="showFormModal"
                :handleShowFormModal="handleShowFormModal"
                @close="handleCloseModal()"
                @success="handleSuccessCreate"
            />
        </div>

        <BaseModal
            :showModal="showDeleteModal"
            :width="'width:432px;'"
        >
            <template #header>
                <div class="delete-confirm-header flex flex-row align-center between">
                    <div class="delete-confirm-header-right flex flex-row align-center">
                        <div class="icon20 icon-warning"></div>
                        <span>Xóa Ca làm việc</span>
                    </div>
                    <div class="delete-confirm-header-left">
                        <div class="icon20 close-icon20 pointer" @click="cancelDelete"></div>
                    </div>
                </div>
            </template>
            <template #body>
                <div class="delete-confirm-body" v-html="deleteMessage">
                </div>
            </template>
            <template #footer>
                <div class="delete-confirm-footer flex flex-row flex-end">
                    <BaseButton
                        type="outline-neutral"
                        @click="cancelDelete"
                    >
                        <template #content>
                            <span>Hủy</span>
                        </template>
                    </BaseButton>
                    <BaseButton
                        type="danger"
                        @click="confirmDelete"
                    >
                        <template #content>
                            <span>Xóa</span>
                        </template>
                    </BaseButton>
                </div>
            </template>
        </BaseModal>
    </div>
</template>

<style scoped>

.shifview{
    
    height: calc(100% - 50px);
}

.shifview-header{
    margin-bottom: 16px;
}

.shifview-header-title{
    font-size: 24px;
    font-weight: 700;
    color: #111827;
}

.shiftview-header-addbtn span{
    font-weight: 600;
    padding-left: .25rem;

}

.shifview-body{
    flex: 1;
    height: 0;
}

.inactive{
    background-color: #fee2e2;
    color: #dc2626;
    width: -moz-fit-content;
    width: fit-content;
    padding: 5px 8px;
    border-radius: 999px;
}

.active{
    background-color: #ebfef6;
    color: #009b71;
    padding: 5px 8px;
    border-radius: 999px;
    width: fit-content;
}

/* Delete confirm */

.delete-confirm-header{
    padding: 16px 16px 0 16px;
    margin-bottom: 16px;
}
.delete-confirm-header-right{
    font-weight: 600;
    color: #111827;
    font-size: 20px;

}

.delete-confirm-body{
    padding: 0 16px 0 16px;
    font-size: 13px;
    max-height: 400px;
    overflow-y: auto;
    font-weight: 400;
    line-height: 20px;
    max-width: 100%;
    overflow-wrap: anywhere;
    margin-bottom: 16px;
}

.delete-confirm-footer{
    padding: 0 16px 16px 16px;
    gap: 8px;
}
</style>