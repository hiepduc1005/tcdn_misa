import { createRouter, createWebHistory } from 'vue-router'
import ShiftView from '../views/ShiftView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: ShiftView,
    },
   
  ],
})

export default router
