<template>
  <div class="coming-soon-container">
    <el-empty description="Tính năng đang trong quá trình phát triển" />
  </div>
</template>

<script setup>
// Có thể thêm script setup nếu cần logic riêng, ví dụ:
// import { ref } from 'vue';
// const message = ref('Tính năng đang trong quá trình phát triển');
</script>

<style scoped>
.coming-soon-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
  background-color: #fff;
  border-radius: 4px; /* Bo góc nhẹ cho đẹp nếu nằm trong khung layout */
}
</style>
