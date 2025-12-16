<script setup>
import { computed, ref } from 'vue'
// 1. Import icon đồng hồ từ thư viện icon của Element Plus
import { Clock } from '@element-plus/icons-vue'
import BaseToolTip from '../tooltip/BaseToolTip.vue';

const props = defineProps({
    modelValue: {
        type: String,
        default: ''
    },
    placeholder: {
        type: String,
        default: 'HH:MM' // Đặt mặc định giống ảnh 2
    },
    required: {
        type: Boolean,
        default: false
    },
    errorMessage: {
        type: String,
        default: 'Trường này không được để trống'
    },
    isError: {
        type: Boolean,
        default: false
    }
})

const emit = defineEmits(['update:modelValue', 'blur', 'change']);
const internalError = ref(false);

const hasError = computed(() => props.isError || internalError.value);

const value = computed({
    get() {
        return props.modelValue;
    },
    set(val) {
        emit('update:modelValue', val);
        if (props.required && val) {
            internalError.value = false;
        }
    }
});

const handleBlur = (e) => {
    emit('blur', e);
    if (props.required && !value.value) {
        internalError.value = true;
    }
}

const handleChange = (val) => {
    emit('change', val);
    if (props.required && val) {
        internalError.value = false;
    }
}
</script>

<template>
    <BaseToolTip
        :placement="'bottom'"
        :showArror="true"
        :disabled="!hasError"
    >
        <template #title>
            <div class="custom-time-select" :class="{ 'is-error': hasError }">
                <el-time-select
                  v-model="value"
                  :max-time="'23:59'"
                  class="my-time-select"
                  start="00:00"
                  step="00:30"
                  end="23:30"
                  :placeholder="props.placeholder"
                  :prefix-icon="null"
                  filterable
                  allow-create    
                  :suffix-icon="Clock"
                  @blur="handleBlur"
                  @change="handleChange"
                />
            </div>
        </template>
        <template #content>
            <span v-if="hasError">{{ errorMessage }}</span>
        </template>
    </BaseToolTip>
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

/* Style lỗi */
.custom-time-select.is-error .el-select__wrapper {
    box-shadow: 0 0 0 1px #ef4444 !important;
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