<script setup>
import { reactive, ref, watch } from 'vue';
import BaseButton from '../components/button/BaseButton.vue';
import ShiftBody from './ShiftBody.vue';
import ShiftFormModal from './ShiftFormModal.vue';
import { COLUMN_TYPE, NUMBER_DATE_FILTER_OPERATORS, SHIFT_MODAL_TYPE, TEXT_FILTER_OPERATORS } from '../constants/common';
import ShiftAPI from '../apis/components/shift/ShiftAPI';
import { formatDate } from '../utils/formatDateFns';
import { camelToPascalCase, formatTimeToHHMM } from '../utils/formatFns';
import BaseToolTip from '../components/tooltip/BaseToolTip.vue';
import BaseModal from '../components/modal/BaseModal.vue';
import { ElMessage } from 'element-plus';


const pagingState = reactive({
  filterItems: [],
  customFilters: [],
  sortItems: [],
  pageSize: 20,
  pageIndex: 1
});
const pagingResult = reactive({
  data: [],
  totalRecord: 0,
  totalPage: 0,
});

const loadDataPagingShift = async () => {
    
    const response = await ShiftAPI.paging(pagingState);
    if(response.errors && response.statusCode >= 400){
        console.error("loadDataPagingShift :" , response.errors);
        return;
    }

    pagingResult.data = response?.data.data?.dataPaging;
    pagingResult.totalPage = response?.data.data?.totalPages;
    pagingResult.totalRecord = response?.data.data?.totalRecords;

}

const handleInactiveAll = async (shiftIds) => {
    if (!Array.isArray(shiftIds)) {
        shiftIds = [shiftIds];
    }
    await ShiftAPI.inactiveAll(shiftIds);
}

const handleActiveAll = async (shiftIds) => {
    if (!Array.isArray(shiftIds)) {
        shiftIds = [shiftIds];
    }
    await ShiftAPI.activeAll(shiftIds);
}


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

const showFormModal = ref(false);
const selectedShift = ref({});
const modalType = ref(SHIFT_MODAL_TYPE.CREATE);

const handleShowFormModal = (type = SHIFT_MODAL_TYPE.CREATE, data = {}) => {
    modalType.value = type;
    selectedShift.value = data;
    showFormModal.value = true;
}

const handleCloseModal = () => {
    showFormModal.value = false;
}

const handleEdit = (row) => {
    handleShowFormModal(SHIFT_MODAL_TYPE.UPDATE, row);
}

const handleDuplicate = (row) => {
    const duplicatedData = { ...row };
    // Xóa định danh để tạo mới
    delete duplicatedData.shiftCode;
    delete duplicatedData.shiftId;
    
    handleShowFormModal(SHIFT_MODAL_TYPE.CREATE, duplicatedData);
}

const handleUpdateStatus = async ({ ids, status }) => {
    pagingResult.data.forEach(item => {
        if (ids.includes(item.shiftId)) {

            // Cập nhập ở frontend
            item.inactive = status;
            item.modifiedDate = new Date().toISOString();
            item.modifiedBy = "Admin";
        }
    });


    try {                                                                                                      
        if (status === false) {                                                                                
            await handleActiveAll(ids);                                                                     
        } else {                                                                                              
            await handleInactiveAll(ids);                                                                   
        }                                                                                                     
    } catch (error) {                                                                                          
        console.error("Lỗi cập nhật trạng thái:", error);                                                      
    }
}

const showDeleteModal = ref(false);
const idsToDelete = ref([]);
const deleteMessage = ref('');

const handleRequestDelete = (shiftIds) => {
    idsToDelete.value = shiftIds.shiftIds;
    if (shiftIds.shiftIds.length === 1) {
        // Tìm tên của ca làm việc cần xóa
        const item = pagingResult.data.find(i => i.shiftId === shiftIds.shiftIds[0]);
        const code = item ? item.shiftCode : '';
        deleteMessage.value = `Ca làm việc <b>${code}</b> sau khi bị xóa sẽ không thể khôi phục. Bạn có muốn tiếp tục xóa không?`;
    } else {
        deleteMessage.value = `Các ca làm việc đã chọn sau khi bị xóa sẽ không thể khôi phục. Bạn có muốn tiếp tục xóa không?`;
    }
    showDeleteModal.value = true;
};

const confirmDelete = async () => {
    try {
        await ShiftAPI.deleteShifts(idsToDelete.value);
        showSuccessToast("Xóa Ca làm việc thành công")
        // Refresh lại dữ liệu sau khi xóa thành công
        await loadDataPagingShift();
        showDeleteModal.value = false;
        idsToDelete.value = [];
    } catch (error) {
        console.error("Lỗi khi xóa:", error);
        // Có thể thêm thông báo lỗi ở đây
    }
};

const cancelDelete = () => {
    showDeleteModal.value = false;
    idsToDelete.value = [];
};
    
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
const handleSuccessCreate = async () => {
    await loadDataPagingShift();
    if(modalType === SHIFT_MODAL_TYPE.CREATE){
        showSuccessToast("Thêm ca làm việc thành công");
    }else showSuccessToast("Sửa ca làm việc thành công");
}

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

const handleChangePageSize = (val) => {
    pagingState.pageSize = val
    pagingState.pageIndex = 1
}

const handleChangePageIndex = (val) => {
    pagingState.pageIndex = val
}


const handleFilterChange = (filters) => {

    pagingState.filterItems = filters.map(filter => {
        return {
            column: camelToPascalCase(filter.field),
            value: filter.value + "",
            operator: filter.operator + ""
        }
    });
}

// Xử lý khi sorts thay đổi
const handleSortChange = (sorts) => {
    pagingState.sortItems = sorts.map(sort => {
        return {
            fieldName: camelToPascalCase(sort.fieldName),
            direction: sort.direction
        }
    })
}

// Xử lý export excel
const handleExportExcel = async () => {
    try {
        const response = await ShiftAPI.exportExcel(pagingState);

        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;

        const contentDisposition = response.headers['content-disposition'];
        let fileName = 'Danh_sach_ca.xlsx'; // Tên mặc định nếu không lấy được
        
        if (contentDisposition) {
            const match = contentDisposition.match(/filename="?([^"]+)"?/);
            if (match && match[1]) {
                fileName = decodeURIComponent(match[1]); // Decode để hiển thị tiếng Việt đúng
            }
        }
        link.setAttribute('download', fileName);

        document.body.appendChild(link);
        link.click();
        
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