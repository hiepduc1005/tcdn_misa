<template>
  <div style="height: 100%;" class="flex flex-column"> 
    <Header></Header>
    <div style="height: 100%;" class="flex flex row"> 
      <Sidebar :menuItems="menuData" />
      <div style="flex: 1; padding: 20px; overflow: hidden; display: flex; flex-direction: column;"> 
        <router-view /> 
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import Sidebar from './layout/Sidebar.vue';
import Header from './layout/Header.vue';
// Định nghĩa Schema Menu
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