<script setup>
import { defineProps, defineEmits } from 'vue';

const props = defineProps({
    row: { type: Object, required: true },            // dữ liệu dòng hiện tại
    actions: { type: Array, default: () => [] }       // danh sách action truyền vào
});

const emit = defineEmits(["action-click"]);

const handleClick = (action) => {
    emit("action-click", action.key, props.row);
};
</script>

<template>
    <div class="action-menu">
        <div 
            v-for="action in actions" 
            :key="action.key"
            class="action-item"
            @click="handleClick(action)"
        >
            <div :class="['icon16', action.iconClass]"></div>
            <span>{{ action.label }}</span>
        </div>
    </div>
</template>

<style scoped>
.action-menu {
    background: white;
    border-radius: 4px;
    padding: 6px 0;
    min-width: 150px;
}

.action-item {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 14px;
    cursor: pointer;
}

.action-item:hover {
    background: #f5f5f5;
}
</style>
