<script setup>
import { ref } from 'vue'
// 1. Import icon đồng hồ từ thư viện icon của Element Plus
import { Clock } from '@element-plus/icons-vue'

const startTime = ref('')

const props = defineProps({
    placeholder: {
        type: String,
        default: 'HH:MM' // Đặt mặc định giống ảnh 2
    }
})
</script>

<template>
  <div class="custom-time-select">
    <el-time-select
      v-model="startTime"
      :max-time="'23:59'"
      class="my-time-select"
      start="00:00"
      step="00:30"
      end="23:30"
      
      :placeholder="props.placeholder"
      
      :prefix-icon="null"    
      :suffix-icon="Clock"   
    />
    </div>
</template>

<style >

.el-select__wrapper{
    max-height: 28px !important; 
    min-height: 28px !important;
}

/* CSS Tùy chỉnh để giống ảnh 2 hơn (Optional) */
.my-time-select {
  width: 100%;
  max-width: 122px;

}

.el-select__wrapper.is-focused {
    box-shadow: 0 0 0 1px #009b71 !important;
}

/* Tùy chọn: Nếu muốn đổi cả màu khi hover */
:deep(.el-input__wrapper:hover) {
   box-shadow: 0 0 0 1px #16a34a inset !important;
}

/* Tùy chỉnh màu placeholder cho giống ảnh */
:deep(.el-input__inner::placeholder) {
  color: #9ca3af; /* Màu xám nhạt */
  font-weight: 500;
}

/* Nếu muốn chỉnh icon đồng hồ to/nhỏ hoặc màu sắc */
:deep(.el-input__suffix-inner .el-icon) {
  color: #6b7280;
  font-size: 16px;
}
</style>