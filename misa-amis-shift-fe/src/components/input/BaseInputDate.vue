<script setup>
import { defineProps, defineEmits } from 'vue';

//#region Props
const props = defineProps({
    // Value cho v-model
    modelValue: {
        type: [String, Number],
        default: null
    },
    // Type của Date Picker ('year' | 'years' |'month' | 'months' | 'date' | 'dates' | 'datetime' | 'week' | 'datetimerange' | 'daterange' | 'monthrange' | 'yearrange')
    type: {
        type: String,
        default: 'date'
    },
    // Text placeholder
    placeholder: {
        type: String,
        default: 'Chọn ngày'
    },
    rangeSeparator: {
        type: String,
        default: '/'
    },
    // Kích thước component (large, default, small)
    size: {
        type: String,
        default: 'default'
    },
    // Format hiển thị trên input (ví dụ: YYYY/MM/DD)
    format: {
        type: String,
        default: 'YYYY-MM-DD'
    },
    // Giá trị thật sự sẽ được bind (ví dụ: timestamp, Date object, hay string)
    valueFormat: {
        type: String,
        default: 'YYYY-MM-DD'
    },
    // Cho phép xóa giá trị hay không
    clearable: {
        type: Boolean,
        default: true
    },
    // Disabled input
    disabled: {
        type: Boolean,
        default: false
    },
    // Style tùy chỉnh (ví dụ: width)
    style: {
        type: [String, Object],
        default: () => ({})
    }
});
//#endregion

const emit = defineEmits(['update:modelValue', 'change']);

// Xử lý sự kiện thay đổi giá trị và emit ra ngoài
const handleChange = (value) => {
    emit('update:modelValue', value);
    emit('change', value);
};
</script>

<template>
    <el-date-picker
        :v-model="modelValue"
        @change="handleChange"
        :range-separator="props.rangeSeparator"
        :type="props.type"
        :placeholder="props.placeholder"
        :size="props.size"
        :format="props.format"
        :value-format="props.valueFormat"
        :clearable="props.clearable"
        :disabled="props.disabled"
        :style="props.style"
        
        class="base-date-input"
    />
</template>

<style scoped>
/* Bạn có thể thêm style tùy chỉnh cho BaseDateInput ở đây */
.base-date-input {
    /* Đảm bảo nó có độ rộng mặc định dễ nhìn */
    width: 100%; 
}
</style>