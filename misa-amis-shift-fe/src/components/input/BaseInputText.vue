<script setup>
import { computed, ref } from 'vue';
import BaseToolTip from '../tooltip/BaseToolTip.vue';

//#region Props
const props = defineProps({
    modelValue: {
        type: [String, Number],
        default: ''
    },
    type: {
        type: String,
        default: 'text'
    },
    placeholder: {
        type: String,
        default: ''
    },
    style: {
        type: [String,Object],
        default:""
    },
    inputColor: {
        type: [String,Object],
        default:""
    },
    
    disabled : {
        type: Boolean,
        default: false
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
//#endregion

const emit = defineEmits(['update:modelValue', 'blur'])
const internalError = ref(false);

const hasError = computed(() => props.isError || internalError.value);

const value = computed({
    get() {
        return props.modelValue;
    },
    set(val) {
        emit('update:modelValue', val);
        if (props.required && val) {
                    console.log(val)

            internalError.value = false;
            hasError.value = false;
        }
    }
});

const handleBlur = (e) => {
    emit('blur', e);
    if (props.required && !value.value) {
        internalError.value = true;
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
            <div 
                :style="style" 
                :class="{ 'disabled': props.disabled, 'is-error': hasError }" 
                class="base-input-container flex pointer"
            >
                <input  
                    class="input fullw" 
                    :type="props.type"
                    :placeholder="props.placeholder"
                    :disabled="disabled"
                    :style="inputColor"
                    v-model="value"
                    @blur="handleBlur"
                />
            </div>
        </template>
        <template #content>
            <span v-if="hasError">{{ errorMessage }}</span>
        </template>
    </BaseToolTip>
</template>

<style scoped>
.base-input-container{
    border: 1px solid #D1D5DB;
    border-radius: 4px;
    background-color: #fff;
    padding: 5px 12px;
    -moz-column-gap: 4px;
    column-gap: 4px;
    align-items: center;
    flex: 1 1 0%;
    min-height: 32px;
    max-height: 32px;
}

.base-input-container:hover{
    border: 1px solid #b7babd;
}

.base-input-container:focus-within{
    border-color: #009b71;
}

.base-input-container.is-error {
    border-color: #ef4444 !important;
}

.base-input-container.is-error:focus-within {
    border-color: #ef4444 !important;
}

.base-input-container input{
    white-space: nowrap;
    border: none;
    padding: 0;
    background: transparent;
    overflow: hidden;
}

/* ... existing styles ... */
input::placeholder {
    color: #9ca3af; /* Màu xám nhạt */
    opacity: 1; /* Đảm bảo không bị mờ (do Firefox mặc định làm mờ placeholder) */
    font-size: 13px; /* Đặt cỡ chữ nếu cần */
    font-weight: 500;
}

/* Các tiền tố trình duyệt cũ */
input::-webkit-input-placeholder { /* Chrome, Edge, Safari */
    color: #9ca3af;
    opacity: 1;
    font-weight: 600;

}

input:-ms-input-placeholder { /* Internet Explorer */
    color: #9ca3af;
    opacity: 1;
}

input::-moz-placeholder { /* Firefox */
    color: #9ca3af;
    opacity: 1;
}
.disabled, input:disabled {
    background-color: #f3f4f6;
    color: #4b5563;
    cursor: default;
}
</style>