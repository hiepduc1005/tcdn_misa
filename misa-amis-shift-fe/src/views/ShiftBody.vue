
<template>
    <div class="body-list flex flex-column">
        <div class="toolbar-list flex flex-row">
            <BaseSearchInput
                :placeholder="'Tìm kiếm'"
            >
            </BaseSearchInput>

            <div v-show="selectedItems.length > 0" class="selected-item-actions flex flex-row align-center">
                <div style="font-size: 13px; font-weight: 600;" class="selected-count">Đã chọn <span style="font-size: 14px;" class="bold">{{ selectedItems?.length }}</span></div>
                <div @click="handleUnselect" class="unselected">Bỏ chọn</div>
                <BaseButton
                    type="success"
                >
                    <template #content>
                        <div class="icon16 active-icon green"></div>
                        <span>Sử dụng</span>
                    </template>
                </BaseButton>
                <BaseButton
                    type="danger-outline"
                >
                    <template #content>
                        <div class="icon16 inactive-icon red"></div>
                        <span>Ngừng xử dụng</span>
                    </template>
                </BaseButton>

                <BaseButton
                    type="danger-outline"
                >
                    <template #content>
                        <div class="icon16 trash-red-icon"></div>
                        <span>Xóa</span>
                    </template>
                </BaseButton>
            </div>

            <div class="reload-button">
                <BaseToolTip
                    :placement="'top'"
                >
                    <template #title>
                        <BaseButton
                            type="default"
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

        </div>
        <div class="content-body flex flex-column fullw flex-1" style="min-height: 0;">
            <div class="table-container fullw flex-1">
                <table class="shif-table">
                    <thead>
                        <tr>
                            <th style="z-index: 100;" class="sticky-left sticky-top">
                                <input 
                                    type="checkbox" 
                                    :checked="isAllSelected"
                                    @change="toggleAll"
                                >
                            </th>
                            <th class="sticky-top" v-for="(col) in props.columns" :key="col.field" :ref="el => columnRefs[col.field] = el">
                                <div class="flex flex-row align-center between">
                                    <BaseToolTip
                                        :placement="'top'"
                                    >
                                        <template #title>
                                            {{ col.label }}
                                        </template>
    
                                        <template #content>
                                            <span>{{ col.label }}</span>
                                        </template>
                                    </BaseToolTip>
    
                                    <div v-if="col.filterable" class="th-title-icon flex align-center fullh" @click="() => handleClickFilter(col.field)">
                                        <div class="icon16 filter-icon" filter-icon></div>
                                    </div>
    
                                    <BaseFilterPopup
                                        v-if="col.filterable"
                                        :parentEl="columnRefs[col.field]"
                                        :showPopup="showPopupField === col.field"
                                        :title="col.label"
                                        @close="showPopupField=''"
                                    />
                                </div>
                            </th>
                            <th style="z-index: 100;" class="sticky-right sticky-top">
                             
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(data,index) in props.datas" :key="index">
                            <td :class="isItemSelected(data.ShiftID) ? 'checked-td' : ''" class="sticky-left">
                                <input 
                                    type="checkbox" 
                                    :checked="isItemSelected(data.ShiftID)"
                                    @change="toggleItem(data.ShiftID)"
                                >
                            </td>
                            <td 
                                class="sticky" 
                                :class="isItemSelected(data.ShiftID) ? 'checked-td' : ''" 
                                v-for="(col ) in props.columns" :key="col.field"
                            >
                                <slot
                                    :name="`cell${col.field}`"
                                    :value="data[col.field]"                                    
                                >
                                    <BaseToolTip
                                        :placement="'bottom-start'"
                                        :showArror="false"
                                    >
                                        <template #title>
                                            {{ data[col.field] }}
                                        </template>

                                        <template #content>
                                            <span>{{ data[col.field] }}</span>
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
                                            <div class="feature-btn">
                                                <div class="icon16 pencil-icon"></div>
                                            </div>
                                        </template>

                                        <template #content>
                                            <span>Sửa</span>
                                        </template>
                                    </BaseToolTip>
                                    
                                    
                                    <div 
                                        class="feature-btn" 
                                        @click="handleClickActionBtn(data.ShiftID)"
                                        :ref="el => actionBtnRefs[data.ShiftID] = el"
                                    >
                                        <div class="icon16 more-feature-icon"></div>
                                    </div>

                                    <BasePopover
                                        :parentEl="actionBtnRefs[data.ShiftID]"
                                        :show="actionBtnId === data.ShiftID"
                                        @close="actionBtnId = null"
                                    >
                                        <template #content>
                                            <BaseActionMenu
                                                :row="data"
                                                :actions="SHIFT_TABLE_ACTION"
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
            <div class="shift-pagination flex flex-row align-center">
                <div class="total-count flex flex-row align-center">
                    <div class="total-label">Tổng số: </div>
                    <div class="total">{{ props.datas?.length }}</div>
                </div>
                <div class="pagination-sticky flex flex-row align-center">
                    <div class="page-size-label">Số dòng/trang</div>
                    <div class="page-size-component">
                        <BaseSelectInput
                            :options="PAGE_SIZE_OPTIONS"
                            :style="'width: 80px'"  
                            :defaultValue="20"
                                                
                            >
                        </BaseSelectInput>
                    </div>
                    <div class="page-info">1 - 17</div>
                    <div class="pagination-btn flex flex-row align-center">
                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="true"
                        >
                            <template #content>
                                <div class="icon16 step-backward-icon disabled-icon"></div>
                            </template>
                        </BaseButton>

                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="true"
                        >
                            <template #content>
                                <div class="icon16 angle-left-icon disabled-icon"></div>
                            </template>
                        </BaseButton>

                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="true"
                        >
                            <template #content>
                                <div class="icon16 angle-right-icon disabled-icon"></div>
                            </template>
                        </BaseButton>

                        <BaseButton
                            :type="'text-neutral'"
                            :disabled="true"
                        >
                            <template #content>
                                <div class="icon16 step-forward-icon disabled-icon"></div>
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
import { computed, reactive, ref } from 'vue';
import BaseToolTip from '../components/tooltip/BaseToolTip.vue';
import BaseButton from '../components/button/BaseButton.vue';
import BaseSearchInput from '../components/input/BaseSearchInput.vue';
import BaseSelectInput from '../components/input/BaseSelectInput.vue';
import { PAGE_SIZE_OPTIONS, SHIFT_TABLE_ACTION } from '../constants/common';
import BaseFilterPopup from '../components/overlay/BaseFilterPopup.vue';
import BasePopover from '../components/overlay/BasePopover.vue';
import BaseActionMenu from '../components/menu/BaseActionMenu.vue';
import ShiftFormModal from './ShiftFormModal.vue';

const props = defineProps({
    datas: {
        type: Array,
        default: []
    },
    columns: {
        type: Array,
        default: []
    }
})


 
const selectedItems = ref([]);
const columnRefs = reactive({});
const showPopupField = ref('');

const actionBtnRefs = reactive({});
const actionBtnId = ref(null);

const handleClickFilter = (field) => {
    showPopupField.value = field;
}

const handleClickActionBtn = (id) => {
    actionBtnId.value = id;
}

const isItemSelected = (id) => {
    return selectedItems.value.includes(id);
};

const toggleItem = (id) => {
    if (isItemSelected(id)) {
        selectedItems.value = selectedItems.value.filter(item => item !== id);
    } else {
        selectedItems.value.push(id);
    }
};

const isAllSelected = computed(() => {
    return props.datas.length > 0 
        && selectedItems.value.length === props.datas.length;
});

const toggleAll = () => {
    if (isAllSelected.value) {
        selectedItems.value = [];
    } else {
        selectedItems.value = props.datas.map(item => item.ShiftID);
    }
};

const handleUnselect = () =>{
    selectedItems.value = [];
}


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

.reload-button{
    margin: auto 0 auto auto;
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
    font-weight: 500;

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
    font-weight: 700;
    color: #111827;
}

.shift-pagination .pagination-sticky{
    gap: 16px;
}

.pagination-sticky .page-info{
    font-weight: 700;
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
</style>