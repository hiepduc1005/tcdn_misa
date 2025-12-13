<script setup>

//#region Props
const props = defineProps({
  type: {
    type: String,
    default: "default", // success | danger-outline | danger | solid-brand
  },
  disabled: {
    type: Boolean,
    default: false
  },
  icon: {
    type: String,
    default: "",
  }
})
//#endregion


const emit = defineEmits(["click"])
</script>

<template>
  <button 
    class="base-button"
    :class="`btn-${props.type}`"
    @click="emit('click')"
    :disabled="props.disabled"
  >
    <slot name="content">      
    </slot>
  </button>
</template>

<style scoped>
.base-button {
    transition: all .2s ease;
    cursor: pointer;
    display: flex;
    align-items: center;
    outline: none;
    padding: 6px 12px;
    border-radius: 4px;
    position: relative;
    font-size: 13px;
    height: 28px;
    background-color: #fff;
    font-weight: 600;
    border-width: 1px;
}

.btn-default{
  border: 1px solid #D1D5DB;
  color: #111827;
  background-color: #fff;
}

.btn-default:hover{
  background-color: #f3f4f6;
}

/* success */

.btn-success {
  border-color: #009b71;
  color: #009b71;
}
.btn-success:hover {
  background-color: #dcfce7;
}

/* danger-outline */

.btn-danger-outline {
  border: 1px solid #dc2626;
  color: #dc2626;

}
.btn-danger-outline:hover {
  background-color: #fee2e2;
}

/* danger */

.btn-danger {
  background-color: #dc2626;
  border-color: #dc2626;
  color: white;
}
.btn-danger:hover {
  opacity: 0.85;
}

/* solid-brand */

.btn-solid-brand {
  background-color: #009b71;
  color: #fff;
  border: none;
}

.btn-solid-brand:hover{
  background-color: #007b5d;
}

/* text-neutral */
.btn-text-neutral{
  color: #111827;
  border: none;
  background-color: transparent;
}


.btn-text-neutral:hover{
  background-color: #e5e7eb;
}


.btn-text-neutral:disabled{
  cursor: not-allowed;
  color: #7b7b7b;
}

/*filled-neutral  */
.btn-filled-neutral{
  background-color: #f3f4f6;
  color: #111827;
  border: none;
}

.btn-filled-neutral:hover{
  background-color: #e5e7eb;
}

/* outline-neutral */

.btn-outline-neutral{
  border: 1px solid #D1D5DB;
  color: #111827;
  background-color: #fff;
}

.btn-outline-neutral:hover{
  background-color: #f3f4f6;
}

</style>
