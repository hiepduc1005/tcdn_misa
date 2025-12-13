<template>
  <div style="height: 100%;" class="flex flex-column"> 
    <Header></Header>
    <div style="height: 100%;" class="flex flex row"> 
      <Sidebar :menuItems="menuData" />
      <div style="flex: 1; padding: 20px; overflow: hidden; display: flex; flex-direction: column;"> 
        <router-view /> 
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import Sidebar from './layout/Sidebar.vue';
import Header from './layout/Header.vue';
// Định nghĩa Schema Menu

</script>