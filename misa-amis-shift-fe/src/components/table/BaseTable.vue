<script lang="ts" setup>
import { computed, PropType, ref } from 'vue'


interface TableField {
  key: string
  label: string
}

interface TableItem {
  id: number
  [key: string]: unknown
}


const props = defineProps({
  fields: {
    type: Array as PropType<TableField[]>,
    default: () => []
  },
  items: {
    type: Array as PropType<TableItem[]>,
    default: () => []
  },
  limit:{
    type: Number,
    default: 25
  },
  page:{
    type: Number,
    default: 1
  },
   handleNextPage: {
    type: Function as PropType<(e: MouseEvent) => void>,
    default: () => {}
  },
   handlePrevPage: {
    type: Function as PropType<(e: MouseEvent) => void>,
    default: () => {}
  },
  canMoveNextPage:{
    type: Function,
    default: () => {}
  },
  canMovePrevPage:{
    type: Function,
    default: () => {}
  },
  onEdit: {
    type: Function,
    default: null
  },
  onDelete: {
    type: Function,
    default: null
  }
})

const selectedItems = ref<number[]>([])

const displayedFieldKeys = computed(() =>
  props.fields.map(f => f.key)
)

function getNestedValue(item: any, key: string) {
  return key.split(/[\.\[\]]/).filter(Boolean).reduce((acc, k) => acc?.[k], item) ?? '';
}

const startItem = computed(() =>{
    return ((props.page - 1 ) * props.limit ) + 1;
})

const endItem = computed(() => {
    return props.page * props.limit;
})

const pagedItems = computed(() => {
  return props.items.slice(startItem.value - 1, endItem.value)
})

const isAllSelected = computed(() => {
  return pagedItems.value.length > 0 && 
         pagedItems.value.every(item => selectedItems.value.includes(item.id))
})

const hasSelectedItems = computed(() => {
  return selectedItems.value.length > 0
})

const toggleAll = () => {
  if (isAllSelected.value) {
    // Bỏ chọn tất cả items trong trang hiện tại
    pagedItems.value.forEach(item => {
      const index = selectedItems.value.indexOf(item.id)
      if (index > -1) {
        selectedItems.value.splice(index, 1)
      }
    })
  } else {
    // Chọn tất cả items trong trang hiện tại
    pagedItems.value.forEach(item => {
      if (!selectedItems.value.includes(item.id)) {
        selectedItems.value.push(item.id)
      }
    })
  }
}

const toggleItem = (itemId: number) => {
  const index = selectedItems.value.indexOf(itemId)
  if (index > -1) {
    selectedItems.value.splice(index, 1)
  } else {
    selectedItems.value.push(itemId)
  }
}

const isItemSelected = (itemId: number) => {
  return selectedItems.value.includes(itemId)
}

const handleDelete = () => {
  if (props.onDelete && selectedItems.value.length > 0) {
    props.onDelete([...selectedItems.value])
    selectedItems.value = []
  }
}

</script>

<template>
    <div class="content__table">
        <div class="table-container">
            <table class="custom-table">
                <thead>
                    <tr>
                        <th class="sticky-left">
                            <input 
                                type="checkbox" 
                                :checked="isAllSelected"
                                @change="toggleAll"
                            >
                        </th>
                        <th v-for="field in fields" :key="field.key">
                            {{ field.label }}
                        </th>
                        <th class="sticky-right"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in pagedItems" :key="item.id">
                        <td class="sticky-left">
                            <input 
                                type="checkbox" 
                                :checked="isItemSelected(item.id)"
                                @change="toggleItem(item.id)"
                            >
                        </td>
                        <td v-for="(key , index) in displayedFieldKeys" :key="index + key">
                            <slot
                                :name="`cell${key}`"
                                :value="getNestedValue(item, key)"
                                :item="item"  
                                  
                            >
                                {{ getNestedValue(item, key) }}
                            </slot>
                        </td>
                        <td class="sticky-right">
                            <svg @click="onEdit?.(item.id)" width="24px" height="24px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M18 10L21 7L17 3L14 6M18 10L8 20H4V16L14 6M18 10L14 6" stroke="#555353" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="table-footer flex flex-row align-center gap12">
            <div v-if="hasSelectedItems" class="delete-button-container">
                <button class="delete-button" @click="handleDelete">
                    <svg width="16px" height="16px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M10 12V17M14 12V17M4 7H20M19 7L18.133 19.142C18.0971 19.6466 17.8713 20.1188 17.5011 20.4636C17.1309 20.8083 16.6439 21 16.138 21H7.862C7.35614 21 6.86907 20.8083 6.49889 20.4636C6.1287 20.1188 5.90292 19.6466 5.867 19.142L5 7H19ZM15 7V4C15 3.73478 14.8946 3.48043 14.7071 3.29289C14.5196 3.10536 14.2652 3 14 3H10C9.73478 3 9.48043 3.10536 9.29289 3.29289C9.10536 3.48043 9 3.73478 9 4V7H15Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    </svg>
                    Xóa ({{ selectedItems.length }})
                </button>
            </div>
            <div class="total-count">
                Tổng: <b>{{ props.items.length ?? 0 }}</b> bản ghi
            </div>
            <div style="margin-right: 14px; margin-left: 8px;" class="">
                Số bản ghi/trang
            </div>
            <div class="limit-dropdown flex flex-row align-center">
                <div>{{ props.limit }}</div>
                <svg width="12px" height="12px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path fill-rule="evenodd" clip-rule="evenodd" d="M12.7071 14.7071C12.3166 15.0976 11.6834 15.0976 11.2929 14.7071L6.29289 9.70711C5.90237 9.31658 5.90237 8.68342 6.29289 8.29289C6.68342 7.90237 7.31658 7.90237 7.70711 8.29289L12 12.5858L16.2929 8.29289C16.6834 7.90237 17.3166 7.90237 17.7071 8.29289C18.0976 8.68342 18.0976 9.31658 17.7071 9.70711L12.7071 14.7071Z" fill="#626060"></path> </g></svg>
            </div>
            <div class="">
                {{ startItem }} - {{ endItem }} bản ghi
            </div>
            <div class="page-nav flex flex-row align-center">
                <div class="page__pre"  :class="{inactive: !canMovePrevPage()}" @click="handlePrevPage">
                    <svg width="16px" height="16px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="#636363"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <polyline fill="none" stroke="#636363" stroke-width="2" points="7 2 17 12 7 22" transform="matrix(-1 0 0 1 24 0)"></polyline> </g></svg>
                </div>
                <div class="page__next" :class="{inactive: !canMoveNextPage()}" @click="handleNextPage">
                    <svg width="16px" height="16px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="#636363" transform="rotate(180)"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <polyline fill="none" stroke="#636363" stroke-width="2" points="7 2 17 12 7 22" transform="matrix(-1 0 0 1 24 0)"></polyline> </g></svg>
                </div>
            </div>
        </div>

    </div>
</template>

<style scoped>
    .content__table {
        width: 100%;
        margin-top: 16px;
    }

    .inactive{
        opacity: 0.5;
    }

    .content__table .table-container {
        width: 100%;
        overflow-x: auto;
        overflow-y: auto;
        border: 1px solid #e5e7eb;
        background: #fff;
        max-height: calc(100vh - 300px);
    }

    .custom-table {
        width: 100%;
        height: 50%;
        border-spacing: 0;
    }

    .custom-table th,
    .custom-table td {
        padding: 12px 16px;
        white-space: nowrap;    
        border-bottom: 1px solid #f1f3f5;
        background: white;
    }

    .custom-table tr:hover .sticky-right svg{
        display: block;
        
    }

    .custom-table tr:hover td{
        background-color: #c7f4f1 !important;
    }

    .custom-table td:hover{
        cursor: pointer;
    }

    .table-footer{
        padding-top: 20px;
    }

    .total-count{
        flex: 1;
    }


    .limit-dropdown{
        border: 1px solid #ccc;
        border-radius: 4px;
        padding: 6px 14px;
        gap: 4px;
        cursor: pointer;
    }

    .limit-dropdown:hover{
        border-color: #3B82F6;
    }

    .page-nav{
        gap: 4px;
    }

    .page__pre{
        cursor: pointer;
    }

    .page__next{
        cursor: pointer;
    }

    .delete-button-container {
        margin-right: auto;
    }

    .delete-button {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 8px 16px;
        background-color: #dc3545;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-size: 14px;
        font-weight: 500;
        transition: background-color 0.2s ease;
    }

    .delete-button:hover {
        background-color: #c82333;
    }

    .delete-button svg {
        stroke: white;
    }
</style>
