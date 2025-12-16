import { createRouter, createWebHistory } from 'vue-router'
import ShiftView from '../views/ShiftView.vue'
import ComingSoon from '../views/ComingSoon/ComingSoon.vue' // Import ComingSoon component

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/masterdata/work-shift',
      name: 'work-shift',
      component: ShiftView,
    },
    // Catch-all route for any undefined paths, redirecting to ComingSoon
    {
      path: '/:pathMatch(.*)*',
      name: 'ComingSoon',
      component: ComingSoon,
    },
  ],
})

export default router
