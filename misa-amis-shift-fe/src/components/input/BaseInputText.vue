<script setup>
//#region Props
const props = defineProps({
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
    disabled : {
        type: Boolean,
        default: false
    }
})
//#endregion
</script>

<template>
    <div 
        :style="style" 
        :class="{ 'disabled': props.disabled }" 
        class="base-input-container flex pointer"
    >
        <input  
            class="input fullw" 
            :type="props.type"
            :placeholder="props.placeholder"
            :disabled="disabled"
        />

    </div>
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


.base-input-container input{
    white-space: nowrap;
    border: none;
    padding: 0;
    background: transparent;
    overflow: hidden;
}

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