<script setup>
import { onBeforeUnmount, ref, watch } from 'vue';

const props = defineProps({
  modelValue: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: 'Tìm kiếm'
  },
  debounceTime: {
    type: Number,
    default: 500 
  }
});

const inputValue = ref(props.modelValue);

let timeout = null;

const emit = defineEmits(['update:modelValue', 'search'])


const handleInput =  (e) => {
    inputValue.value = e.target.value;
    
    if(timeout){
        clearTimeout(timeout);
    }
    
    timeout = setTimeout(() => {
        emit('update:modelValue',inputValue.value);
        emit('search',inputValue.value);
    },props.debounceTime)
}

watch(() => props.modelValue, (newValue) => {
    if(newValue !== inputValue.value){
        inputValue.value = newValue;
    }
})

onBeforeUnmount(() => {
    if(timeout) clearTimeout(timeout);
})

</script>

<template>
    <div class="search-container">
        <div class="icon-wrapper">
            <slot name="icon">
                <div class="icon16 search-icon pointer"></div>
            </slot>
        </div>
        <input
            class="search-input"
            type="text"
            :placeholder="placeholder"
            :value="inputValue"
            @input="handleInput"
        />
    </div>
</template>

<style scoped>
.search-container {
    display: flex;
    align-items: center;
    width: 100%;
    transition: all .2s cubic-bezier(.4, 0, .2, 1);
    width: 200px;
    background-color: #fff;
    padding: 5px 12px;
    -moz-column-gap: 4px;
    column-gap: 4px;
    border: 1px solid #D1D5DB;
    border-radius: 4px;
    
}

.search-container:hover{
    border-color: #a9acb0;
}

.icon-wrapper {
  left: 10px;
  display: flex;
  align-items: center;
  
}

.search-input{
    border: none;
    background: transparent;
    white-space: nowrap;
    overflow: hidden;
    background: #fff;
    display: flex;
    width: 100%;
    color: #111827;
    font-size: 13px;
    font-weight: 500;
}



.search-input::placeholder {
    color: #757575;
    font-size: 13px;
    font-weight: 600;
}

/* Hiệu ứng khi focus vào ô input */
.search-container:focus-within {
    border-color: #009b71;
}
</style>