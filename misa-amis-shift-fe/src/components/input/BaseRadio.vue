<template>
  <label class="base-radio">
    <input
      type="radio"
      :value="value"
      :name="inputName"
      v-model="model"
    />
    <span class="radio-custom"></span>
    <span class="radio-label"><slot></slot></span>
  </label>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  modelValue: [String, Number, Boolean],
  value: [String, Number, Boolean],
  inputName: String
});

const emit = defineEmits(["update:modelValue"]);

const model = computed({
  get() {
    return props.modelValue;
  },
  set(val) {
    emit("update:modelValue", val);
  }
});
</script>

<style scoped>
.base-radio {
  display: inline-flex;
  align-items: center;
  cursor: pointer;
  margin-right: 20px;
  user-select: none;
}

.base-radio input {
  display: none;
}

.radio-custom {
  width: 16px;
  height: 16px;
  min-width: 16px;
  min-height: 16px;
  border-radius: 50%;
  border: 1px solid #7b7b7b;
  display: inline-block;
  position: relative;
  margin-right: 8px;
  cursor: pointer;
}

/* ✔ Đổi border khi checked */
.base-radio input:checked + .radio-custom {
  border-color: #009b71;
}

/* ✔ Chấm xanh bên trong */
.base-radio input:checked + .radio-custom::after {
  content: "";
  width: 8px;
  height: 8px;
  background-color: #009b71;
  border-radius: 50%;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}


.radio-label {
  font-size: 13px;
  display: inline-block;
  font-weight: 600;
}
</style>
