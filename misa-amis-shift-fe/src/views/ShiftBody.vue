
<template>
    <div class="body-list flex flex-column">
        <div class="toolbar-list flex flex-row">
            
            <div class="flex align-center">
                <!-- Search shift -->
                <BaseSearchInput
                    :placeholder="'Tìm kiếm'"
                    :debounceTime="500"
                    @search ="(val) => emit('search',val)"
                >
                </BaseSearchInput>
            </div>

            <!-- Select action  -->
            <div v-show="selectedItems.length > 0" class="selected-item-actions flex flex-row align-center">
                <div style="font-size: 13px; font-weight: 600;" class="selected-count">Đã chọn <span style="font-size: 14px;" class="bold">{{ selectedItems?.length }}</span></div>
                <div @click="handleUnselect" class="unselected">Bỏ chọn</div>
                <BaseButton
                    v-if="hasInactiveItemSelected"
                    type="success"
                    @click="handleUpdateStatus(false)"
                >
                    <template #content>
                        <div class="icon16 active-icon green"></div>
                        <span>Sử dụng</span>
                    </template>
                </BaseButton>
                <BaseButton
                    v-if="hasActiveItemSelected"
                    type="danger-outline"
                    @click="handleUpdateStatus(true)"
                >
                    <template #content>
                        <div class="icon16 inactive-icon red"></div>
                        <span>Ngừng xử dụng</span>
                    </template>
                </BaseButton>

                <BaseButton
                    type="danger-outline"
                    @click="handleDelete()"
                >
                    <template #content>
                        <div class="icon16 trash-red-icon"></div>
                        <span>Xóa</span>
                    </template>
                </BaseButton>
            </div>

            <!-- Filter box  -->
            <div class="filter-conditions fullh" v-show="currentFilters.length > 0 && selectedItems.length === 0">
                <div 
                    class="filter-item" 
                    v-for="filterItem in currentFilters"
                    :key="filterItem.field"
                >
                    <div class="lable-filter-value flex flex-row">
                        <span>{{ getColumnLabel(filterItem) }}</span>
                        <span style="color: #009B71">{{ getOperatorLabel(filterItem) }}</span>
                        <span>{{ filterItem.value }}</span>
                    </div>
                    <div 
                        class="icon16 close-icon pointer" 
                        @click="() => handleClearFilter(filterItem.field)"
                        >
                    </div>
                </div>
                <div class="delete-all-filter" @click="handleClearAllFilter">Bỏ lọc</div>
            </div>

            <div class="button-right flex flex-row">
                <!-- Reload button -->
                <div v-show="selectedItems.length <= 0" class="reload-button">
                    <BaseToolTip
                        :placement="'top'"
                    >
                        <template #title>
                            <BaseButton
                                type="default"
                                @click="handleRefresh"
                            >
                                <template #content>
                                    <div class="icon16 reload-icon"></div>
                                </template>
                            </BaseButton>
                        </template>
    
                        <template #content>
                            <span>Lấy lại dữ liệu</span>
                        </template>
                    </BaseToolTip>
                </div>
    
                <!-- Export button -->
                <div v-show="selectedItems.length <= 0" class="export-button">
                    <BaseToolTip
                        :placement="'top'"
                    >
                        <template #title>
                            <BaseButton
                                type="default"
                                @click="handleExportExcel"
                            >
                                <template #content>
                                    <div class="icon16 export-icon"></div>
                                </template>
                            </BaseButton>
                        </template>
    
                        <template #content>
                            <span>Xuất Excel</span>
                        </template>
                    </BaseToolTip>
                </div>
            </div>

        </div>
        <div class="content-body flex flex-column fullw flex-1" style="min-height: 0;">
            <div class="table-container fullw flex-1">
                <!-- Shift Table -->
                <table class="shif-table">
                    <thead>
                        <tr>
                            <th style="z-index: 100;" class="sticky-left sticky-top checkbox-col">
                                <div class="flex align-center fullh">
                                    <input 
                                        type="checkbox" 
                                        :checked="isAllSelected"
                                        @change="toggleAll"
                                    >
                                </div>
                            </th>
                            <th class="sticky-top" v-for="(col) in props.columns" :key="col.field" :ref="el => columnRefs[col.field] = el">
                                <div class="flex flex-row align-center between">
                                    <BaseToolTip
                                        :placement="'top'"
                                    >
                                        <template #title>
                                            <div 
                                                class="fullw flex align-center" 
                                                :class="col.right ? 'flex-end' : ''"
                                                @click="() => handlePopupSortClick(col.field)"
                                            >
                                                {{ col.label }}
                                                <div v-if="getSortKey(col.field) === 'ASC'" style="margin: 0;" class="icon16 arrow-up-icon"></div>
                                                <div v-if="getSortKey(col.field) === 'DESC'" style="margin: 0;" class="icon16 arrow-down-icon"></div>

                                            </div>

                                            <!-- Sort popup -->
                                            
                                        </template>
                                        
                                        <template #content>
                                            <span>{{ col.label }}</span>
                                        </template>
                                    </BaseToolTip>
                                    <BaseSortMenu 
                                        :showPopup="showPopupSort === col.field"
                                        :parentEl="columnRefs[col.field]"
                                        :field="col.field"
                                        :actionKey="getSortKey(col.field)"
                                        @close="showPopupSort = ''"
                                        @action-key="handleApplySort"

                                    />
    
                                    <div 
                                        v-if="col.filterable"
                                        :class="{'th-title-icon': !isFilterActive(col.field) }"
                                        class="flex align-center fullh"
                                        @click="() => handleClickFilter(col.field)"
                                    >

                                        <div class="icon16" :class="[isFilterActive(col.field) ? 'filter-icon-active' : 'filter-icon' ]"></div>
                                    </div>

                                    <!-- Filter popup -->
                                    <BaseFilterPopup
                                        v-if="col.filterable"
                                        :parentEl="columnRefs[col.field]"
                                        :showPopup="showPopupField === col.field"
                                        :title="col.label"
                                        :type="col.type"
                                        :options="col.options"
                                        :field="col.field"
                                        :currentFilter="getFilter(col.field)"
                                        @close="showPopupField=''"
                                        @apply-filter="handleApplyFilter"
                                        @clear="handleClearFilter"
                                    />
                                </div>
                            </th>
                            <th style="z-index: 100;" class="sticky-right sticky-top">
                             
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Skeleton Loading Rows -->
                        <template v-if="props.isLoading">
                            <SkeletonTableRow
                                v-for="n in (props.pageSize || 10)"
                                :key="`skeleton-${n}`"
                                :columns="props.columns"
                            />
                        </template>
                        <tr v-else v-for="(data,index) in props.datas" :key="index" @dblclick="emit('edit', data)">
                            <td :class="isItemSelected(data.shiftId) ? 'checked-td' : ''" class="sticky-left">
                                <input 
                                    type="checkbox" 
                                    :checked="isItemSelected(data.shiftId)"
                                    @change="toggleItem(data.shiftId)"
                                >
                            </td>
                            <td 
                                class="sticky" 
                                :class="isItemSelected(data.shiftId) ? 'checked-td' : ''" 
                                v-for="(col ) in props.columns" :key="col.field"
                            >
                                <slot
                                    :name="`body-${col.field}`"
                                    :value="data[col.field]"
                                    :data="data"
                                    :index="index"
                                >
                                    <BaseToolTip
                                        :placement="'bottom-start'"
                                        :showArror="false"
                                    >
                                        <template #title>
                                            <div class="fw400 fullw flex align-center" :class="col.right ? 'flex-end' : ''" >
                                                {{ (data[col.field] || data[col.field] == 0)  ? data[col.field] : "-" }}
                                            </div>
                                        </template>

                                        <template #content>
                                            <span>{{  (data[col.field] || data[col.field] == 0) ? data[col.field] : "-" }}</span>
                                        </template>
                                    </BaseToolTip>
                                </slot>
                            </td>
                            <td class="sticky-right">
                                <div class="action-buttons flex flex-row align-center">
                                    <BaseToolTip
                                        :placement="'top'"                                       
                                    >
                                        <template #title>
                                            <div 
                                                class="feature-btn"
                                                @click="emit('edit', data)"
                                            >
                                                <div class="icon16 pencil-icon"></div>
                                            </div>                                          
                                        </template>

                                        <template #content>
                                            <span>Sửa</span>
                                        </template>
                                    </BaseToolTip>
                                    
                                    
                                    <div 
                                        class="feature-btn" 
                                        @click="handleClickActionBtn(data.shiftId)"
                                        :ref="el => actionBtnRefs[data.shiftId] = el"
                                    >
                                        <div class="icon16 more-feature-icon"></div>
                                    </div>

                                    <BasePopover
                                        :parentEl="actionBtnRefs[data.shiftId]"
                                        :show="actionBtnId === data.shiftId"
                                        @close="actionBtnId = null"
                                    >
                                        <template #content>
                                            <BaseActionMenu
                                                :row="data"
                                                @action-click="handleMenuActionClick"
                                            >
                                            </BaseActionMenu>
                                        </template>
                                    </BasePopover>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!-- Pagination -->
            <div class="shift-pagination flex flex-row align-center">
                <!-- Total record -->
                <div class="total-count flex flex-row align-center">
                    <div class="total-label">Tổng số: </div>
                    <div class="total">{{ props?.totalRecord }}</div>
                </div>
                <div class="pagination-sticky flex flex-row align-center">
                    <div class="page-size-label">Số dòng/trang</div>
                    <div class="page-size-component">
                        <!-- Page size select -->
                        <BaseSelectInput
                            :options="PAGE_SIZE_OPTIONS"
                            :style="'width: 80px'"  
                            :defaultValue="props?.pageSize"
                            @select="handleChangePageSize"                  
                            >
                        </BaseSelectInput>
                    </div>
                    <div class="page-info">{{ pageInfo }}</div>
                    <!-- Pagination navigate -->
                    <div class="pagination-btn flex flex-row align-center">
                        <!-- Move to first page -->
                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="!canMovePrePage"
                            @click="emit('page-index',1)"
                        >
                            <template #content>
                                <div class="icon16 step-backward-icon" :class="!canMovePrePage ? 'disabled-icon' : ''"></div>
                            </template>
                        </BaseButton>

                        <!-- Move to prev page -->
                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="!canMovePrePage"
                            @click="handlePrevPage"
                        >
                            <template #content>
                                <div class="icon16 angle-left-icon" :class="!canMovePrePage ? 'disabled-icon' : ''"></div>
                            </template>
                        </BaseButton>

                        <!-- Move to next page -->
                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="!canMoveNextPage"
                            @click="handleNextPage"
                        >
                            <template #content>
                                <div class="icon16 angle-right-icon" :class="!canMoveNextPage ? 'disabled-icon' : ''"></div>
                            </template>
                        </BaseButton>

                        <!-- Move to last page -->
                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="!canMoveNextPage"
                            @click="emit('page-index',props.totalPage)"
                        >
                            <template #content>
                                <div class="icon16 step-forward-icon" :class="!canMoveNextPage ? 'disabled-icon' : ''"></div>
                            </template>
                        </BaseButton>

                    </div>
                </div>
            </div>
        </div>
        <ShiftFormModal />
    </div>
</template>

<script setup>
    
import { computed, reactive, ref, registerRuntimeCompiler, watch } from 'vue';
import BaseToolTip from '../components/tooltip/BaseToolTip.vue';
import BaseButton from '../components/button/BaseButton.vue';
import BaseSearchInput from '../components/input/BaseSearchInput.vue';
import BaseSelectInput from '../components/input/BaseSelectInput.vue';
import BaseFilterPopup from '../components/overlay/BaseFilterPopup.vue';
import BasePopover from '../components/overlay/BasePopover.vue';
import BaseActionMenu from '../components/menu/BaseActionMenu.vue';
import ShiftFormModal from './ShiftFormModal.vue';
import SkeletonTableRow from '../components/skeleton/SkeletonTableRow.vue';
import { COLUMN_TYPE, PAGE_SIZE_OPTIONS } from '../constants/common';
import { formatTimeToHHMM } from '../utils/formatFns';
import BaseSortMenu from '../components/menu/BaseSortMenu.vue';
const emit = defineEmits(['page-index','page-size','close', 'update-status', 'request-delete', 'edit', 'duplicate' , 'search', 'refresh','filter','export']);

const props = defineProps({
    datas: {
        type: Array,
        default: []
    },
    columns: {
        type: Array,
        default: []
    },
    pageIndex: {
        type:  Number,
        default: 1
    },
    pageSize: {
        type:  Number,
        default: 20
    },
    totalPage: {
        type:  Number,
        default: 0
    },
    totalRecord: {
        type:  Number,
        default: 0
    },
    isLoading: {
        type: Boolean,
        default: false
    }
})

// Danh sách ca làm việc được check
const selectedItems = ref([]);
const columnRefs = {};

// Popup filter
const showPopupField = ref('');

//Popup sort
const showPopupSort = ref('')

const actionBtnRefs = {};
const actionBtnId = ref(null);

// Lưu trữ filter hiện tại
const currentFilters = ref([]);

// Lưu trữ sort hiện tại
const currentSorts = ref([])

/**
 * Hàm emit sự kiện duplicate và đóng action menu lên component cha
 *
 * @author hiepnd
 */
const handleDuplicate = (data) => {
    emit('duplicate', data);
    actionBtnId.value = null
    
}

/**
 * Hàm emit sự kiện page-size để thay đổi số bản ghi/trang lên component cha
 *
 * @author hiepnd
 */
const handleChangePageSize = (size) => {
    emit("page-size",size)
}

/**
 * Định dạng cho phần page info vd: 21 - 40
 *
 * @author hiepnd
 */
const pageInfo = computed(() => {
    if (props.totalRecord === 0) return "0 - 0";
    console.log(props.pageSize )
    const start = (props.pageIndex - 1) * props.pageSize + 1;
    const end = Math.min(props.pageIndex * props.pageSize, props.totalRecord);

    return `${start} - ${end}`;
});

/**
 * Biến kiểm tra nếu như có thể lùi page xuống 
 *
 * @author hiepnd
 */
const canMovePrePage = computed(() => {
    return props.pageIndex > 1;
});

/**
 * Biến kiểm tra nếu như page có thể tiến lên
 *
 * @author hiepnd
 */
const canMoveNextPage = computed(() => {
    return props.pageIndex < props.totalPage;
});


/**
 * Hàm emit update-status lên component cha để update status của ca làm việc theo danh sách id
 *
 * @author hiepnd
 */
const handleUpdateStatus = (newStatus,shiftId = null) => {
    // Nếu như có shiftId là đang chọn bằng menu action 
    if(shiftId){
        emit('update-status', { ids: [shiftId], status: newStatus });
    }else{
        emit('update-status', { ids: [...selectedItems.value], status: newStatus });
    }

    actionBtnId.value = null // Đóng menu nếu mở
    selectedItems.value = []; // Bỏ chọn sau khi update
}

/**
 * Hàm emit request-delete lên component cha để xóa ca làm việc theo danh sách id
 *
 * @author hiepnd
 */
const handleDelete = (shiftId = null) =>{
    if(shiftId){
        emit('request-delete', { shiftIds: [shiftId]});
        selectedItems.value = selectedItems.value.filter(id => id !== shiftId);
    }else{
        emit('request-delete', { shiftIds: [...selectedItems.value]});
        selectedItems.value = [];
    }

    // Đóng menu nếu mở
    actionBtnId.value = null
}

/**
 * Hàm emit refresh lên component cha để refresh theo danh sách ca làm việc
 *
 * @author hiepnd
 */
const handleRefresh = () => {
    emit('refresh');
}

/**
 * Hàm emit export lên component cha để export theo danh sách ca làm việc hiện tại
 *
 * @author hiepnd
 */
const handleExportExcel = () => {
    emit('export');
}

/**
 * Hàm để mở popup filter cho cột theo field 
 *
 * @author hiepnd
 */
const handleClickFilter = (field) => {
    showPopupField.value = field;
}

/**
 * Hàm để mở popup menu action ca làm việc theo id
 *
 * @author hiepnd
 */
const handleClickActionBtn = (id) => {
    actionBtnId.value = id;
}

/**
 * Hàm kiểm tra nếu id truyền vào đã được check
 *
 * @author hiepnd
 */
const isItemSelected = (id) => {
    return selectedItems.value.includes(id);
};

/**
 * Hàm để check và uncheck row theo id ca làm việc
 *
 * @author hiepnd
 */
const toggleItem = (id) => {
    if (isItemSelected(id)) {
        selectedItems.value = selectedItems.value.filter(item => item !== id);
    } else {
        selectedItems.value.push(id);
    }
};

/**
 * Biến để kiểm tra xem tất cả danh sách trong trang hiện tại đã được check hay chưa
 *
 * @author hiepnd
 */
const isAllSelected = computed(() => {
    if (props.datas.length === 0) return false;
    return props.datas.every(item => selectedItems.value.includes(item.shiftId));
});

/**
 * Hàm để check và uncheck tất cả ca làm việc trong trang hiện tại
 *
 * @author hiepnd
 */
const toggleAll = () => {
    if (isAllSelected.value) {
        const currentIds = props.datas.map(item => item.shiftId);
        selectedItems.value = selectedItems.value.filter(id => !currentIds.includes(id));
    } else {
        const currentIds = props.datas.map(item => item.shiftId);
        const newIds = currentIds.filter(id => !selectedItems.value.includes(id));
        selectedItems.value = [...selectedItems.value, ...newIds];
    }
};

/**
 * Hàm để uncheck tất cả ca làm việc trong trang hiện tại
 *
 * @author hiepnd
 */
const handleUnselect = () =>{
    selectedItems.value = [];
}

/**
 * Biến để kiểm tra xem trong danh sách ca làm việc đã check 
 * có ca làm việc nào có inactive = true (Ngừng sử dụng)
 *
 * @author hiepnd
 */
const hasInactiveItemSelected = computed(() => {
    const selectedRows = props.datas.filter(item => selectedItems.value.includes(item.shiftId));
    // Hiện nút "Sử dụng" nếu có ít nhất 1 dòng đang Ngừng hoạt động (inactive = true)
    return selectedRows.some(item => item.inactive === true || item.Inactive === true);
});

/**
 * Biến để kiểm tra xem trong danh sách ca làm việc đã check 
 * có ca làm việc nào có inactive = false (Đang sử dụng)
 *
 * @author hiepnd
 */
const hasActiveItemSelected = computed(() => {
    const selectedRows = props.datas.filter(item => selectedItems.value.includes(item.shiftId));
    // Hiện nút "Ngừng sử dụng" nếu có ít nhất 1 dòng đang Hoạt động (inactive = false)
    return selectedRows.some(item => !item.inactive && !item.Inactive);
});

/**
 * Hàm để emit các sự kiện của menu action bao gồm
 * duplicate (Nhân bản), toggle_inactive (Ngừng sử dụng)
 * toggle_active (Sử dụng), delete (Xóa)
 *
 * @author hiepnd
 */
const handleMenuActionClick = (key, data) => {
    switch (key) {

        case "duplicate":
            handleDuplicate(data);
            break;

        case "toggle_inactive":
            handleUpdateStatus(true,data.shiftId);
            break;

        case "toggle_active":
            handleUpdateStatus(false,data.shiftId);
            break;

        case "delete":
            handleDelete(data.shiftId);
            break;

        default:
            console.warn("Unknown action:", key);
    }
};

/**
 * Hàm để lùi page đi 1 so với page hiện tại 
 *
 * @author hiepnd
 */
const handlePrevPage = () => {
    if (canMovePrePage.value) {
        emit("page-index", props.pageIndex - 1);
    }
};

/**
 * Hàm để tăng page thêm 1 so với page hiện tại 
 *
 * @author hiepnd
 */
const handleNextPage = () => {
    if (canMoveNextPage.value) {
        emit("page-index", props.pageIndex + 1);
    }
    
};


// Filter logic

// Emit filter nếu như currentFilters thay đổi
watch(currentFilters, (newFilters) => {
    emit('filter', newFilters);
},{deep:true})

// Áp dụng filter
const handleApplyFilter = (newFilter) => {    
    // Kiểm tra xem đã có filter này chưa
    const existingFilterIndex = currentFilters.value.findIndex((filter) => filter.field === newFilter.field);

    if(existingFilterIndex !== -1){

        // Nếu như người dùng chọn trống hoặc k trống thì k cần nhập (bỏ qua điều kiện value trống)
        if(newFilter.operator === 'not_empty' || newFilter.operator === 'empty'){
            currentFilters.value[existingFilterIndex] = newFilter;
        }

        // Nếu đã có filter trạng thái thì cập nhập (bỏ qua điều kiện value trống)
        else if(newFilter.field === 'inactive' && newFilter.operator !== ''){
            currentFilters.value[existingFilterIndex] = newFilter;
        }

        // Nếu có filter nhưng k có operator và value thì sẽ xóa filter đó
        else if(newFilter?.value == null || newFilter?.value === ''  || newFilter?.operator === ''){
            currentFilters.value.splice(existingFilterIndex,1)
        }
        // Đã có filter và có value mới thì cập nhập
        else{
        
            currentFilters.value[existingFilterIndex] = newFilter;
        }

    }
    // Nếu như chưa có filter này thì sẽ thêm mới 
    else {
        // Nếu như người dùng chọn trống hoặc k trống thì k cần nhập (bỏ qua điều kiện value trống)
        if(newFilter.operator === 'not_empty' || newFilter.operator === 'empty'){
            currentFilters.value.push(newFilter);
        }

        // Nếu chưa có filter trạng thái thì thêm vào (bỏ qua điều kiện value trống)
        else if(newFilter.field === 'inactive' && newFilter.operator !== ''){
            currentFilters.value.push(newFilter);
        }
        else if(newFilter.operator !== '' && newFilter.value !== ''){
            currentFilters.value.push(newFilter);
        }
    } 

    //Đóng popup filter
    showPopupField.value = '';

}

// Xóa filter khi nhấn bỏ chọn trong popup
const handleClearFilter = (field) => {
    //Lấy vị trí filter cần xóa trong mảng currentFilters
    const filterToClearIndex = currentFilters.value.findIndex((filter) => filter.field === field);
    
    // Nếu như tồn tại thì xóa 
    if(filterToClearIndex !== -1){
        currentFilters.value.splice(filterToClearIndex,1)
    }

    // Đóng popup filter
    showPopupField.value = '';
}

// Xóa tất cả filter
const handleClearAllFilter = () => {
    currentFilters.value = [] 
}

// Kiểm tra xem 1 cột đang có filter hay không
const isFilterActive = (field) => {
    const filterToClearIndex = currentFilters.value.findIndex((filter) => filter.field === field);
    
    // Nếu k có là false
    if(filterToClearIndex === -1){
        return false
    }

    //Nếu có là true
    return true
}

// Lấy label hiển thị cho operator (vd: eq -> Bằng)
const getOperatorLabel = (filterItem) => {

    // Lấy cột
    const column = props.columns.find((col) => col.field === filterItem.field);

    if(column && column.options){
        // Lấy option của filter item
        const option = column.options.find((opt) => opt.value === filterItem.operator)
        if(option){
            return option.label;
        }

        // Nếu k tìm thấy option thì trả về value
        return filterItem.operator
    }
}

// Lấy label hiển thị cho cột (vd: shiftCode -> Mã ca)
const getColumnLabel = (filterItem) => {

    // Lấy cột của filter item
    const column = props.columns.find((col) => col.field === filterItem.field);

    // Nếu như k có cột thì trả về field của filter item hiện tại
    return column ? column.label : filterItem.field;
}

// Lấy filter dựa theo field
const getFilter = (field) => {
    return currentFilters.value.find((filter) => filter.field === field);
}

// Sort logic 

const handlePopupSortClick = (field) => {
    showPopupSort.value = field
}

const handleApplySort = (key,field) => {
    const currentSortIndex = currentSorts.value.findIndex(sort => sort.fieldName === field);
    // Nếu như có sort
    if(currentSortIndex !== -1){
        
        // Nếu như k sort thì xóa sort item trong danh sách
        if(key === 'none'){
            currentSorts.value.splice(currentSortIndex,1);

            // Đóng sort menu
            showPopupSort.value = '';
            return;
        }

        // Cập nhập key
        currentSorts.value[currentSortIndex].direction = key
    }
    // Nếu chưa có thì thêm mới
    else {
        currentSorts.value.push({
            fieldName: field,
            direction: key
        })
    }

    // Đóng sort menu
    showPopupSort.value = '';
}


// Lấy sort dựa theo field name
const getSortKey = (field) => {
    return currentSorts.value.find(sort => sort.fieldName === field)?.direction;
}

// Theo dõi currentSorts để emit lên component cha
watch(currentSorts, (newSorts) => {
    emit('sort', newSorts);
},{deep:true})

</script>

<style  scoped>

.body-list{
    background-color: #fff;
    border-radius: 4px;
    overflow: hidden;
    height: 100%;
}

.body-list .toolbar-list{
    padding: 8px 16px;
    background: #fff;
    border-top-left-radius: 4px;
    border-top-right-radius: 4px;
    gap: 8px;
}

.body-list .selected-item-actions{
    background-color: #fff;
    gap: 8px;
    margin-left: 8px;
    height: 28px;
    flex-shrink: 0;
}

.selected-item-actions .unselected{
    color: #009b71;
    cursor: pointer;
    margin: 0 8px;
    font-size: 13px;
    font-weight: 600;
}
.selected-item-actions .unselected:hover{
    text-decoration: underline;
}

.button-right{
    margin: auto 0 auto auto;
    gap: 8px;
}

.table-container {
    overflow: auto;
}



/* Custom Scrollbar for table-container */
.table-container::-webkit-scrollbar {
    width: 6px;
    height: 6px;
}

.table-container::-webkit-scrollbar-thumb {
    background-color: #888;
    border-radius: 3px;
}

.table-container::-webkit-scrollbar-thumb:hover {
    background-color: #555;
}

.table-container::-webkit-scrollbar-track {
    background-color: #f1f1f1;
    border-radius: 3px;
}

/* Tabel  */

.shif-table {
    width: max-content;
    border-collapse: separate;
    border-spacing: 0;
}

.shif-table thead th {
    top: 0;
    z-index: 1;
    background-color: #F3F4F6;
    color: #111827;
    font-weight: 400;
    text-align: left;
    padding: 0 16px;
    height: 34px;
    border-bottom: 1px solid #D1D5DB;
    white-space: nowrap;
    max-width: 250px;
    cursor: pointer;
    border-right: 1px solid #D1D5DB;
    font-size: 13px;
    font-weight: 600;
}

.shif-table tbody td {
    padding: 0 16px;
    height: 32px;
    vertical-align: middle;
    background-color: #fff;
    border-bottom: 1px solid #E5E7EB;
    max-width: 250px;
    overflow: hidden;
    font-size: 13px;
    font-weight: 400;

}
.shif-table .action-buttons{
    justify-content: flex-start;
    padding-left: 8px;
    align-items: center;
    gap: 8px;
}

.shif-table tbody tr:hover td {
    background-color: #e5e7eb !important;
}

.checked-td {
    background-color: #a4f6d3 !important;
}

.th-title-icon {
    opacity: 0;
    margin-left: 12px;
}

.shif-table thead th:hover .th-title-icon{
    opacity: 1;
}

/* Paginate */

.shift-pagination{
    height: 44px;
    min-height: 44px;
    align-items: center;
    background-color: #f3f4f6;
    justify-content: space-between;
    padding: 8px 16px;
    border-radius: 0 0 4px 4px;
    width: 100%;
}
.shift-pagination .total-label{
    color: #262626;
}

.shift-pagination .total{
    margin-left: 4px;
    font-weight: 500;
    color: #111827;
    flex-shrink: 0;
    font-size: 13px;
}

.shift-pagination .pagination-sticky{
    gap: 16px;
    flex-shrink: 0;
}

.pagination-sticky .page-info{
    font-weight: 600;
    font-size: 13px;
}

/* Input */

input[type="checkbox"] {
    accent-color: #009b71;
    width: 16px;
    height: 16px;
    cursor: pointer;
    transition: all .2s ease;
    transition-timing-function: cubic-bezier(.4, 0, .2, 1);
}

/* Filter conditions */

.filter-conditions{
    display: flex;
    align-items: center;
    row-gap: 4px;
    flex-wrap: wrap;
    /* margin-bottom: 8px; */
    margin-right: 8px;
    max-height: 56px;
    overflow-y: auto;
}
.filter-item{
    display: flex;
    gap: 8px;
    height: 24px;
    padding: 0 8px;
    border-radius: 4px;
    position: relative;
    margin-right: 8px;
    white-space: normal;
    align-items: center;
    background-color: #f3f4f6;
    max-width: calc(100% - 8px);
}

.lable-filter-value{
    gap: 8px;
}

.delete-all-filter{
    display: inline-block;
    color: #f06666;
    cursor: pointer;
    white-space: nowrap;
}

.delete-all-filter:hover{
    text-decoration: underline;
}
</style>