<script setup>
import { ref } from 'vue';


const props = defineProps({
    options: {
        type: Array,
        default: () => []
    },
    style: {
        type: [String,Object],
        default: ''
    },
    placeholder: {
        type: String,
        default: '',
    },
    defaultFirstOption: {
        type: Boolean,
        default: false
    },
    size:{
        type: String,
        default: null
    },
    filterable: {
        type: Boolean,
        default: false
    },
    defaultValue: {
        type: [String,Number],
        default: ''
    }
})

const selectValue = ref(props.defaultValue);

const emit = defineEmits(['select']);

const handleSelect = (val) => {
    emit("select",val);
}
</script>

<template>
    <el-select 
        v-model="selectValue" 
        :placeholder="props.placeholder" 
        :style="props.style"
        :default-first-option="defaultFirstOption"
        :filterable="filterable"
        :size="props.size ? props.size : ''"
        @change="handleSelect"
    >
        <el-option
            v-for="item in props.options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
        />
  </el-select>
</template>

<style>

.el-select-dropdown__item.is-selected {
    color: #009b71;
    background-color: #d0fbe7;
}

.el-select-dropdown__item.is-selected::after {
    content: "";
    border-right: 2px solid #009b71;
    border-bottom: 2px solid #009b71;
    transform: rotate(45deg);
    width: 5px;
    height: 10px;
    position: absolute;
    right: 12px;
    top: 50%;
    margin-top: -6px;
}

.el-select-dropdown__wrap {
    /* Ví dụ: Giới hạn chiều cao tối đa là 250px */
    max-height: 200px !important; 
    /* Dùng !important nếu các style mặc định của Element Plus bị ghi đè */
}

</style>