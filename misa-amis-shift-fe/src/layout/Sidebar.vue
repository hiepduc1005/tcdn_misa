<template>
    <div class="sidebar flex flex-column" :class="{ 'collapsed': isCollapse }">
        <div class="sidebar__body">
            <div 
                v-for="item in menuItems"
                :key="item?.index"
                class="sidebar__item-wrapper"
                @mouseenter="onMouseEnterItem(item, $event)" 
                @mouseleave="onMouseLeave"
                >
                <div v-if="item.type === 'line' " class="sidebar__item-line"></div>
                <slot name="item" :item="item">
                    <div v-if="item.type !== 'line'" class="sidebar__item flex flex-row pointer" :class="(openMenu === item.index) ? 'sidebar__item-active' : ''" @click="item.type !== 'popout' ? handleClickNav(item) : null">
                        <div  class="item__icon">
                            <div class="icon" :class="[item.iconClass,(openMenu === item.index) ? 'bg-white' : '']"></div>
                        </div>
                        <div class="item__content">
                            <span>{{ item.title }}</span>
                        </div>
                        <div v-if="item?.type === 'dropdown'" class="flex flex-end align-center flex-1">
                            <transition name="fade-slide" mode="out-in">
                                <div v-if="openMenu !== item.index" key="down" :class="[(openMenu === item.index) ? 'bg-white' : '']" class="icon icon_dropdown"></div>
                                <div v-else key="up" :class="[(openMenu === item.index) ? 'bg-white' : '']" class="icon icon_dropdown-up"></div>
                            </transition>
                        </div>

                        <div v-if="item?.type === 'popout'" class="flex flex-end align-center flex-1">
                            <div key="pop" class="icon icon-popout"></div>
                        </div>

                    </div>
                    <div 
                        v-show="item?.type === 'dropdown'"
                        class="sidebar__children"
                        :class="{ 'open': openMenu === item.index }"
                    >
                        <div 
                            v-for="child in item.children" 
                            :key="child.index" 
                            class="sidebar__child-item"
                            @mouseenter="onMouseEnterItem(child, $event)" 
                            @mouseleave="onMouseLeave"
                        >
                            <div class="icon icon-turndown"></div>
                            <span>{{ child.title }}</span>
                            <div v-if="child?.type === 'popout'" class="flex flex-end align-center flex-1">
                                <div key="pop" class="icon icon-popout"></div>
                            </div>
                            <BasePopout 
                                :onMouseEnterPopout="onMouseEnterPopout"
                                :onMouseLeave="onMouseLeave"
                                :item="child"
                                :hoveringIndex="hoveringIndex"
                                :popoutStyle="popoutStyle"
                            />
                        </div>
                    </div>

                    <BasePopout 
                        :onMouseEnterPopout="onMouseEnterPopout"
                        :onMouseLeave="onMouseLeave"
                        :item="item"
                        :hoveringIndex="hoveringIndex"
                        :popoutStyle="popoutStyle"
                    />

                    

                </slot>
            </div>
        </div>
        <div class="sidebar_collapse">
            <div class="button-collapse flex flex-row content-center align-center" @click="toggleCollapse">
                <div class="icon icon-collapse" :style="isCollapse ? 'transform: rotate(180deg)' : ''"></div>
                
                <span style="margin-left: 8px;">Collapse</span>
            </div>
        </div>
    </div>

    
</template>

<script setup>
import { ref, computed, reactive, watch } from 'vue'; // Thêm reactive
import { useRoute } from 'vue-router';
import BasePopout from '../components/overlay/BasePopout.vue';
import { STORAGE_KEY } from '../constants/common';
import { getDataFromLocalStorage, saveDataToLocalStorage } from '../utils/localStorageFns';

const props = defineProps({
  menuItems: {
    type: Array,
    required: true,
    default: () => []
  }
});

const openMenu = ref(null);
const route = useRoute();
const isCollapse = ref(getDataFromLocalStorage(STORAGE_KEY));

// Lưu index của item đang được hover
const hoveringIndex = ref(null);

// Lưu vị trí top/left
const popoutStyle = reactive({
    top: '0px',
    left: '0px'
});

// Biến timeout để xử lý độ trễ khi di chuột
let hoverTimeout = null;

// 1. Khi chuột vào Item cha
const onMouseEnterItem = (item, event) => {
    if(item.type !== 'popout'){
        return;
    }
    // Nếu đang có lệnh đóng (do vừa rời khỏi item khác), hủy nó đi
    if (hoverTimeout) clearTimeout(hoverTimeout);
    
    // Tính toán vị trí
    const rect = event.currentTarget.getBoundingClientRect();
    popoutStyle.top = `${rect.top - 40}px`;
    popoutStyle.left = `${rect.right + 20}px`; 
    hoveringIndex.value = item.index;
    console.log(hoveringIndex.value)
};

// 2. Khi chuột rời khỏi Item cha HOẶC rời khỏi Popout
const onMouseLeave = () => {
    hoverTimeout = setTimeout(() => {
        hoveringIndex.value = null;
    }, 300);
};

// 3. Khi chuột đi vào Popout (đang nằm ở body)
const onMouseEnterPopout = () => {
    if (hoverTimeout) clearTimeout(hoverTimeout);
};

// --- HẾT LOGIC POPOUT ---

const handleClickNav = (item) => {

    if (openMenu.value === item.index) {
        openMenu.value = null; 
    } else {
        openMenu.value = item.index;
    }

};

const activeMenu = computed(() => route.path);
const toggleCollapse = () => {
  isCollapse.value = !isCollapse.value; 
};

watch(isCollapse, (newValue) => {
    saveDataToLocalStorage(STORAGE_KEY, newValue);
});


</script>

<style scoped>
    .sidebar{
        display: flex;
        flex-direction: column;
        width: 238px;
        background: #111827;
        transition: width .2s;
        justify-content: space-between;
       
        overflow-y: overlay;
        overflow-x: hidden;
        height: calc(100vh - 48px);
        flex-shrink: 0;
    }

    .sidebar__body{
        display: flex;
        flex-direction: column;
        gap: 4px;
        padding: 12px 12px 0;
    }

    .sidebar__item {
        color: #9ca3af;
        position: relative;
        padding: 9px 0;
        height: 36px;
        font-size: 13px;
        display: flex;
        align-items: center;
        -moz-column-gap: 8px;
        column-gap: 8px;
        width: 100%;
        cursor: pointer;
        border-radius: 4px;
    }

    
    .sidebar__item-line{
        margin: 8px auto;
        width: 88%;
        border-bottom: 1px solid rgba(209, 213, 219, .3);
    }

    .sidebar__item:hover{
        background-color: #252c3b;
        color: #fff;
        
    }

    .sidebar__item:hover .icon{
        background-color: #fff;
    }

    .sidebar__item span{
        font-weight: 600;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        max-width: 170px;
    }

    .sidebar__item-active{
        background-color: #009b71 !important;
        color: #fff;
    }

    .sidebar__children {
        max-height: 0;
        opacity: 0;                 /* thêm */
        overflow: hidden;
        transition: 
        max-height 0.35s ease-in-out,
        opacity 0.3s ease-in-out;     /* thêm */
    }

    /* Khi mở */
    .sidebar__children.open {
        max-height: 500px;
        opacity: 1;                 /* thêm */
    }

    .sidebar__child-item{
        color: #9ca3af;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        font-size: 13px;
        font-weight: 600;
        max-height: 36px;
        padding: 9px 0;
        cursor: pointer;
        border-radius: 4px;
        display: flex;
        align-items: center;
        width: 100%;
    }

    .sidebar__child-item span{
        padding-left: 8px;
    }

    .sidebar__child-item:hover{
        background-color: #252c3b;
        color: #fff;
    }

    .sidebar__child-item:hover .icon-turndown{
        background-color: #fff;
    }


    .fade-slide-enter-active,
    .fade-slide-leave-active {
    transition: all 0.2s ease;
    }
    .fade-slide-enter-from,
    .fade-slide-leave-to {
    opacity: 0.5;
    }
    .fade-slide-enter-to,
    .fade-slide-leave-from {
    opacity: 1;
    }

    /* Popout */

    .sidebar__item-wrapper {
        position: relative; 

    }

    .sidebar__popout {
        position: fixed; /* Bắt buộc là Fixed khi dùng Teleport */
        z-index: 99999;  /* Cao hẳn lên để đè lên mọi thứ */
        background: #111827;
        border-radius: 4px;
        box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.5), 0 4px 6px -2px rgba(0, 0, 0, 0.3);
        border: 1px solid #374151;
        animation: popoutFadeIn 0.2s ease-out forwards;        
        pointer-events: auto; 
        width: auto;
    }

    .sidebar__item-wrapper:hover .sidebar__popout {
        opacity: 1;
        visibility: visible;
        transform: translateX(0);
    }

    .popout__header {
        background-color: #111827; 
        padding: 12px 16px;
        color: #fff;
        font-weight: 600;
        font-size: 14px;
        display: flex;
        align-items: center;
        gap: 10px;
    }
    .popout__header .icon {
        background-color: #fff; 
    }

    .popout__body {
        padding: 6px 6px;
        column-gap: 8px;
        gap: 4px;
    }

    .popout__body .label{
        height: 32px;
        color: #4b5563;
        font-weight: 500;
        font-size: 13px;
        width: 100%;
        line-height: 14px;
        padding: 0 32px;
        margin-bottom: 4px;
    }

    .popout__item {
        color: #9ca3af;
        font-size: 13px;
        transition: background-color 0.2s;
        cursor: default;
    }

    .body-item{
        border-radius: 4px;
    }

    .body-item:hover{
        background-color: #252c3b;
        color: #fff;
        cursor: pointer;
    }

    .body-item:hover .icon{
        background-color: #fff;
    }

    /* Sidebar collapse */

    .sidebar_collapse{
        box-sizing: border-box;
        width: 100%;
        height: 56px;
        padding: 10px;
        display: flex;
        align-items: center;
        background: #111827;
        border-top: 1px solid rgba(209, 213, 219, .3);
    }

    .button-collapse{
        width: 100%;
        height: 36px;
        border-radius: 4px;
        min-height: 36px;
        background: #111827;
        color: #9ca3af;
        z-index: 2;
        display: flex;
        justify-content: center;
        align-items: center;
        padding-right: 5px;
    }

    .button-collapse:hover{
        cursor: pointer;
        background-color: #252c3b;
        color: #fff;
    }

    .button-collapse:hover .icon{
        background-color: #fff;
    }

    /* Colapse */

    /* 1. Thu nhỏ chiều rộng sidebar */
    .sidebar.collapsed {
        width: 60px; /* Độ rộng khi thu nhỏ (chỉ đủ chứa icon) */
        padding: 0;
    }

    .sidebar.collapsed .sidebar__body {
        /* Đảm bảo padding ngang giảm mạnh */
        padding: 12px 6px 0; /* Đổi 12px thành 6px (hoặc 4px) để icon sát hơn */
    }

    /* 3. Sửa lỗi Item Line (Thanh phân cách) */
    .sidebar.collapsed .sidebar__item-line {
        /* Khi collapse, cố định độ rộng cho thanh ngang và căn giữa */
        width: 80%; /* Giảm width nhỏ hơn 88% để phù hợp với padding 6px */
        margin-left: auto;
        margin-right: auto;
    }

    /* 2. Ẩn các thành phần text và icon phụ */
    .sidebar.collapsed .item__content span,
    .sidebar.collapsed .button-collapse span
    {
        display: none; /* Ẩn hoàn toàn */
    }

    /* Ẩn toàn bộ div chứa icon dropdown/popout khi collapse */
    .sidebar.collapsed .sidebar__item > .flex-1 {
        display: none;
    }

    /* 3. Căn giữa icon chính */
    .sidebar.collapsed .sidebar__item {
        justify-content: center; /* Căn giữa icon */
        padding: 9px 0;
        justify-content: center; 
        /* Bổ sung: Đảm bảo item chiếm 100% width để căn giữa hoạt động đúng */
        width: 100%;
    }

    .sidebar.collapsed .item__icon {
        /* Đảm bảo div bao icon không chiếm hết chỗ */
        flex: none; 
        margin: 0;
    }

    /* 4. Xử lý nút Collapse ở dưới đáy */
    .sidebar.collapsed .button-collapse {
        justify-content: center; /* Căn giữa icon collapse */
    }

    /* 5. Quan trọng: Ẩn menu con dạng Dropdown (Accordion) khi thu nhỏ */
    /* Vì sidebar bé quá không thể hiển thị dropdown xổ xuống được */
    .sidebar.collapsed .sidebar__children {
        display: none !important;
    }

    /* Scroll bar */

    .sidebar::-webkit-scrollbar {
        width: 6px; /* Độ rộng của thanh cuộn */
        background: transparent; /* Màu nền thanh cuộn bình thường (vô hình) */
    }

    /* Hiệu ứng khi thanh cuộn được hover */
    .sidebar:hover::-webkit-scrollbar {
        width: 6px; 
        background: transparent;
    }

    /* Thanh trượt (thumb) */
    .sidebar::-webkit-scrollbar-thumb {
        /* Mặc định thanh trượt sẽ ẩn hoặc rất mờ khi không tương tác */
        background-color: transparent; 
        border-radius: 6px;
    }

    /* Thanh trượt khi hover vào Sidebar (Hình 2) */
    .sidebar:hover::-webkit-scrollbar-thumb {
        /* Khi hover vào sidebar, thanh trượt hiện màu xám (giống màu bạn muốn) */
        background-color: #6b7280; /* Màu xám, có thể điều chỉnh theo ý bạn */
    }

    /* Thanh trượt khi hover trực tiếp vào chính thanh trượt (thumb) */
    .sidebar::-webkit-scrollbar-thumb:hover {
        /* Có thể làm cho màu sáng hơn một chút khi hover trực tiếp vào thumb */
        background-color: #9ca3af; 
    }

    /* Cần thêm thuộc tính overflow-y: auto/overlay nếu chưa có */
    .sidebar {
        /* Đảm bảo sidebar có thể cuộn */
        overflow-y: auto; 
    }


</style>