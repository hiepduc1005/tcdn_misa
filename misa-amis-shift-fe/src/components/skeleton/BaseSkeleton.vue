<template>
  <div class="base-skeleton"></div>
</template>

<script>
    
</script>

<style scoped>
.base-skeleton {
  width: 100%;
  height: 1.2em; /* Adjusted for single line height, can be made a prop */
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  border-radius: 4px;
  animation: shimmer 1.5s infinite;
}

@keyframes shimmer {
  0% {
    background-position: -200% 0;
  }
  100% {
    background-position: 200% 0;
  }
}
</style>
