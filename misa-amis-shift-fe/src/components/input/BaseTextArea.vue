<script setup>
import { computed } from 'vue';

// Định nghĩa các props cho component
const props = defineProps({
    // Giá trị v-model
    modelValue: {
        type: [String, Number],
        default: ''
    },
    // Placeholder hint
    placeholder: {
        type: String,
        default: ''
    },
    // Số dòng hiển thị mặc định
    rows: {
        type: [String, Number],
        default: 3
    },
    // Trạng thái vô hiệu hóa
    disabled: {
        type: Boolean,
        default: false
    },
    // Trạng thái chỉ đọc
    readonly: {
        type: Boolean,
        default: false
    },
    maxLength: {
        type: Number,
        default: "400"
    }
});

// Định nghĩa sự kiện emit
const emit = defineEmits(['update:modelValue']);

// Sử dụng computed để xử lý v-model hai chiều
const value = computed({
    get() {
        return props.modelValue;
    },
    set(val) {
        emit('update:modelValue', val);
    }
});
</script>

<template>
    <textarea
        v-model="value"
        class="base-textarea"
        :placeholder="props.placeholder"
        :rows="props.rows"
        :disabled="props.disabled"
        :readonly="props.readonly"
        :class="{ 'is-disabled': props.disabled }"
        :maxlength="props.maxLength"
    ></textarea>
</template>

<style scoped>
.base-textarea {
    width: 100%; /* Chiếm hết chiều rộng container */
    padding: 8px 12px; /* Padding trong thoải mái */
    
    
    color: #111827; 
    font-size: 14px; /* Cỡ chữ thông dụng */
    line-height: 1.5; /* Khoảng cách dòng dễ đọc */
    
    outline: none; /* Loại bỏ outline mặc định của trình duyệt */
    resize: none; /* Chỉ cho phép kéo giãn theo chiều dọc */
    transition: border-color 0.2s, box-shadow 0.2s; /* Hiệu ứng chuyển màu mượt mà */
    background: #fff;
    height: 68px;
    overflow: auto;
    border-radius: 4px;
    border: 1px solid #d5dfe2;
    padding: 6px 10px;
    resize: none;
}

/* Trạng thái Hover (khi chưa disabled) */
.base-textarea:hover:not(:disabled):not(:focus) {
    border-color: #9ca3af; /* Viền tối hơn một chút khi di chuột */
}

/* Trạng thái Focus - GIỐNG NHƯ TRONG ẢNH */
.base-textarea:focus {
    border-color: #009b71; /* Màu viền xanh brand khi focus */
    /* box-shadow: 0 0 0 1px #009b71; Thêm shadow nhẹ nếu muốn nổi bật hơn */
}

/* Trạng thái Disabled */
.base-textarea:disabled, .base-textarea.is-disabled {
    background-color: #F3F4F6; /* Nền xám */
    color: #9CA3AF; /* Chữ xám */
    cursor: not-allowed; /* Con trỏ cấm */
    border-color: #E5E7EB; /* Viền xám nhạt */
}

/* Style cho Placeholder */
.base-textarea::placeholder {
    color: #9CA3AF; /* Màu placeholder xám chuẩn */
}
</style>