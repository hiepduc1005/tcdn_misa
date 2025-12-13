<template>
    <div class="sidebar flex flex-column" :class="{ 'collapsed': isCollapse }">
        <div class="sidebar__body">
            <div 
                v-for="item in menuData"
                :key="item?.index"
                class="sidebar__item-wrapper"
                @mouseenter="onMouseEnterItem(item, $event)" 
                @mouseleave="onMouseLeave"
                >
                <div v-if="item.type === 'line' " class="sidebar__item-line"></div>
                <slot name="item" :item="item">
                    <div v-if="item.type !== 'line'" class="sidebar__item flex flex-row pointer" :class="[(openMenu === item.index || isActive(item)) ? 'sidebar__item-active' : '']" @click="item.type !== 'popout' ? handleClickNav(item) : null">
                        <div  class="item__icon">
                            <div class="icon" :class="[item.iconClass,(openMenu === item.index || isActive(item)) ? 'bg-white' : '']"></div>
                        </div>
                        <div class="item__content">
                            <span>{{ item.title }}</span>
                        </div>
                        <div v-if="item?.type === 'dropdown'" class="flex flex-end align-center flex-1">
                            <transition name="fade-slide" mode="out-in">
                                <div v-if="openMenu !== item.index" key="down" :class="[(openMenu === item.index || isActive(item)) ? 'bg-white' : '']" class="icon icon_dropdown"></div>
                                <div v-else key="up" :class="[(openMenu === item.index || isActive(item)) ? 'bg-white' : '']" class="icon icon_dropdown-up"></div>
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
                
                <span style="margin-left: 8px;">Thu gọn</span>
            </div>
        </div>
    </div>

    
</template>

<script setup>
import { ref, computed, reactive, watch } from 'vue'; // Thêm reactive
import { useRoute, useRouter } from 'vue-router';
import BasePopout from '../components/overlay/BasePopout.vue';
import { STORAGE_KEY } from '../constants/common';
import { getDataFromLocalStorage, saveDataToLocalStorage } from '../utils/localStorageFns';

const props = defineProps({
 
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
const router = useRouter(); // Thêm dòng này

const handleClickNav = (item) => {
    if (item.path) {
        router.push(item.path);
    }

    if (item.type === 'dropdown') {
        if (openMenu.value === item.index) {
            openMenu.value = null; 
        } else {
            openMenu.value = item.index;
        }
    }
};

const handleChildClickNav = (child) => {
    if (child.path) {
        router.push(child.path);
    }
}
const activeMenu = computed(() => route.path);

const isActive = (item) => {
    // Helper check 1 path cụ thể
    const checkPath = (path) => {
        if (!path) return false;
        if (path === '/' && route.path !== '/') return false;
        return route.path === path || route.path.startsWith(path + '/');
    };

    // 1. Check chính nó
    if (checkPath(item.path)) return true;

    // 2. Check con (dropdown hoặc popout)
    if (item.children && item.children.length > 0) {
        return item.children.some(child => {
            // Trường hợp Dropdown: child có path trực tiếp
            if (checkPath(child.path)) return true;

            // Trường hợp Popout: child có mảng items
            if (child.items && child.items.length > 0) {
                return child.items.some(subItem => checkPath(subItem.path));
            }

            return false;
        });
    }

    return false;
};

const toggleCollapse = () => {
  isCollapse.value = !isCollapse.value; 
};

watch(isCollapse, (newValue) => {
    saveDataToLocalStorage(STORAGE_KEY, newValue);
});

const menuData = ref([
  { 
    index: '1', 
    title: 'Tổng quan',
    name: "tongquan",
    iconClass: 'icon-dashboard',
    path: '/dashboard',
    type: null
  },
  {
    index: "2",
    title: 'Đơn đặt hàng',
    name: "order",
    iconClass: 'icon-saleorder',
    path: "/sale-order",
    type: null
  },
  {
    index: '3',
    name: "kehoachsanxuat",
    title: 'Kế hoạch sản xuất',
    iconClass: 'icon-produce',
    type: 'dropdown',
    children: [
      { index: '3-1', title: 'Kế hoạch tổng thể', path: '/production-plan/overall-plan' },
      { index: '3-2', title: 'Kế hoạch chi tiết', path: '/production-plan/detail-plan' },
      { index: '3-3', title: 'Kế hoạch nguyên vật liệu', path: '/production-plan/materialplan' },
      { index: '3-4', title: 'Kế hoạch mua NVL', path: '/production-plan/materialPurchaseRequest' },
    ]
  },
  {
    index: '4',
    name: "dieuphoithucthi",
    title: 'Điều phối và thực thi',
    iconClass: 'icon-cordinate',
    type: 'dropdown',
    children: [
      { index: '4-1', title: 'Lệnh sản xuất', path: '/production-execution/order' },
      { index: '4-2', title: 'Lịch sản xuất', path: '/production-execution/schedule' },
      { index: '4-3', title: 'Yêu cầu xuất vật tư', path: '/production-execution/material-request' },
      { index: '4-4', title: 'Thống kê sản xuất', path: '/production-execution/statistics' },
      { index: '4-5', title: 'Yêu cầu nhập thành phẩm', path: '/production-execution/fg-receipt' },
    ]
  },
  {
    index: '5',
    name: "checkquality",
    title: 'Kiểm tra chất lượng',
    iconClass: 'icon-checkquality',
    type: 'dropdown',
    children: [
      { index: '5-1', title: 'Yêu cầu kiểm tra', path: '/quality/request' },
      { index: '5-2', title: 'Phiếu kiểm tra', path: '/quality/sheet' },
      { 
        index: '5-3', 
        title: 'Tiêu chuẩn', 
        path: '/quality/standard',
        type: 'popout',
        children: [
          { 
            key: "tieu_chuan",
            items: [
              { index: '5-3-1', title: 'Tiêu chí chất lượng', path: '/quality/criteria' },
              { index: '5-3-2', title: 'Nhóm tiêu chí chất lượng', path: '/quality/criteria-group' },
              { index: '5-3-3', title: 'Bộ tiêu chuẩn kiểm tra chất lượng', path: '/quality/inspection-standard' },
              { index: '5-3-4', title: 'Phương pháp chọn mẫu', path: '/quality/sampling-method' },
              { index: '5-3-5', title: 'Lỗi kiểm tra chất lượng', path: '/quality/inspection-error' },
              { index: '5-3-6', title: 'Nhóm lỗi kiểm tra chất lượng', path: '/quality/error-group' },
            ],
          },
        ]
      },       
    ]
  },
  {
    index: '6',
    name: "khovattu",
    title: 'Kho vật tư',
    iconClass: 'icon-warehouse',
    type: 'dropdown',
    children: [
      { index: '6-1', title: 'Đề nghị kho cấp vật tư', path: '/warehouse/material-request' },
      { index: '6-2', title: 'Nhập kho', path: '/warehouse/receipt' },
      { index: '6-3', title: 'Xuất kho', path: '/warehouse/issue' },
      { index: '6-4', title: 'Điều chuyển', path: '/warehouse/transfer' },
      { index: '6-5', title: 'Tồn kho đầu kỳ', path: '/warehouse/opening-stock' },
    ]
  },
  {
    index: '7',
    name: "eeee",
    type: 'line'
  },
  {
    index: '8',
    name: "report",
    iconClass: 'icon-report',
    title: 'Báo cáo', 
    type: null,
    path: '/reportlist',
  },
  {
    index: '9',
    name: "ee32e",
    type: 'line'
  },
  {
    index: '10',
    name: "spnvl",
    title: 'Sản phẩm, NVL',
    iconClass: 'icon-cube',
    type: 'dropdown',
    children: [
      { index: '10-1', title: 'Vật tư hàng hóa', path: '/masterdata/material' },
      { index: '10-2', title: 'Nhóm vật tư hàng hóa', path: '/masterdata/material-group' },
      { index: '10-3', title: 'Định mức nguyên vật liệu', path: '/masterdata/material-norm' },
      { index: '10-4', title: 'Nguyên vật liệu thay thế', path: '/masterdata/substitute-material' },
    ]
  },
  {
    index: '11',
    name: "quytrinhsx",
    title: 'Quy trình sản xuất',
    iconClass: 'icon-process',
    type: 'dropdown',
    children: [
      { index: '11-1', title: 'Công đoạn', path: '/product-process/stage' },
      { index: '11-2', title: 'Quy trình sản xuất', path: '/product-process/process' },
    ]
  },
  {
    index: '12',
    name: "capacity",
    title: 'Năng lực sản xuất',
    iconClass: 'icon-capatity',
    type: 'dropdown',
    children: [
      { index: '12-1', title: 'Tổ sản xuất', path: '/product-capacity/productionTeam' },
      { index: '12-2', title: 'Máy móc', path: '/product-capacity/robot' },
      { index: '12-3', title: 'Nhóm năng lực', path: '/product-capacity/capacity' },
    ]
  },
  {
    index: '13',
    title: 'Danh mục khác',
    name: "danhmuckhac",
    iconClass: 'icon-category',
    type: 'popout',
    children: [
      { 
        key: "doituong",
        label: "Đối tượng",
        items: [
          { index: '13-1', title: 'Khách hàng', path: '/masterdata/customer' },
          { index: '13-2', title: 'Nhân viên', path: '/masterdata/employee' },
          { index: '13-3', title: 'Đối tượng tập hợp chi phí', path: '/masterdata/cost-object' },
        ],
      },
      { 
        key: "lichlamviec",
        label: "Lịch làm việc",
        items: [
          { index: '13-4', title: 'Ca làm việc', path: '/masterdata/work-shift' },
          { index: '13-5', title: 'Ngày nghỉ', path: '/masterdata/day-off' },
          { index: '13-6', title: 'Lịch làm việc', path: '/masterdata/work-schedule' },
        ],
      },
      { 
        key: "khac",
        label: "Khác",
        items: [
          { index: '13-7', title: 'Cơ cấu tổ chức', path: '/masterdata/organization-structure' },
          { index: '13-8', title: 'Kho', path: '/masterdata/warehouse' },
          { index: '13-9', title: 'Đơn vị tính', path: '/masterdata/unit' },
          { index: '13-10', title: 'Lý do dừng công việc', path: '/masterdata/stop-reason' },
        ],
      },
    ]
  },
  {
    index: '14',
    title: 'Thiết lập',
    name: "settings",
    iconClass: 'icon-setting',
    type: 'popout',
    children: [
      { 
        key: "thietlap",
        label: "Thiết lập",
        items: [
          { index: '14-1', title: 'Thông tin công ty', path: '/system/company-info' },
          { index: '14-2', title: 'Người dùng, vai trò', path: '/system/user-role' },
          { index: '14-3', title: 'Tùy chọn', path: '/system/preferences' },
          { index: '14-4', title: 'Kết nối ứng dụng', path: '/system/app-connections' },
        ],
      },
      { 
        key: "tienich",
        label: "Tiện ích",
        items: [
          { index: '14-5', title: 'Nhật ký truy cập', path: '/system/access-log' },
        ],
      },
    ]
  },
]);
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
        font-weight: 400;
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
        font-weight: 400;
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
        font-weight: 400;
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
        font-weight: 400;
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