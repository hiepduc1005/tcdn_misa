<template>
  <tr class="skeleton-row">
    <!-- Cột checkbox (Sticky Left) -->
    <td class="sticky-left">
      <BaseSkeleton style="width: 16px; height: 16px; border-radius: 2px;" />
    </td>
    
    <!-- Các cột dữ liệu -->
    <td v-for="(col, index) in columns" :key="index">
      <BaseSkeleton />
    </td>

    <!-- Cột hành động (Sticky Right) -->
    <td class="sticky-right">
       <div class="action-skeleton">
          <BaseSkeleton style="width: 20px; height: 20px; border-radius: 4px;" />
          <BaseSkeleton style="width: 20px; height: 20px; border-radius: 4px;" />
       </div>
    </td>
  </tr>
</template>

<script setup>
import BaseSkeleton from './BaseSkeleton.vue';

defineProps({
  columns: {
    type: Array,
    default: () => []
  }
});
</script>

<style scoped>
td {
    padding: 0 16px;
    height: 48px; /* Chiều cao hàng tiêu chuẩn */
    vertical-align: middle;
    background-color: #fff;
    border-bottom: 1px solid #E5E7EB;
}

.sticky-left {
    position: sticky;
    left: 0;
    z-index: 2;
    background-color: #fff;
}

.sticky-right {
    position: sticky;
    right: 0;
    z-index: 2;
    background-color: #fff;
    text-align: center;
}

.action-skeleton {
    display: flex;
    gap: 8px;
    justify-content: center;
}
</style>
