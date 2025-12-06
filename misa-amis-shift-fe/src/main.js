import { createApp } from 'vue'
import App from './App.vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import '../src/assets/css/main.css'
import '../src/assets/css/icon.css'
import router from './router'

const app = createApp(App)
app.use(router);
app.use(ElementPlus)
app.mount('#app')