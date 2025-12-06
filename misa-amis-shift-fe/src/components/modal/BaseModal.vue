<script setup>

const props = defineProps({
    showModal: {
        type: Boolean,
        default: false
    },
    style: {
        type: [String,Object],
        default: ""
    },
    width: {
        type: String,
        default: ''
    }
})

</script>

<template>
    <div v-show="props.showModal" class="overlay">
        <transition name="slide-down">
            <div 
                v-if="props.showModal"
                :style="props.style"
                class="modal-container absolute"
            >
                <div :style="width" class="modal-content absolute">
                    <div class="modal-header">
                        <slot name="header"/>
                    </div>
                    <div class="model-body">
                        <slot name="body"/>
                    </div>
                    <div class="modal-footer">
                        <slot name="footer"/>
                    </div>
                </div>
            </div>
        </transition>
    </div>
</template>

<style scoped>
.overlay {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: rgb(0 0 0 / 25%);
    z-index: 1000; 
}

.modal-container{
    display: flex;
    justify-content: center;
    align-items: center;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
}

.modal-content{
    position: absolute;
    display: flex;
    flex-direction: column;
    border-radius: 4px;
    background: #fff;
    transition: all .3s ease-in-out;
}


/* Animation */

.slide-down-enter-from {
  opacity: 0;
  transform: translateY(-20px);
}

/* Khi đã xuất hiện xong */
.slide-down-enter-to {
  opacity: 1;
  transform: translateY(0);
}

/* Animation trong quá trình xuất hiện */
.slide-down-enter-active {
  transition: all 0.25s ease;
}

/* Khi bắt đầu biến mất */
.slide-down-leave-from {
  opacity: 1;
  transform: translateY(0);
}

/* Khi biến mất hoàn toàn */
.slide-down-leave-to {
  opacity: 0;
  transform: translateY(-20px);
}

/* Animation trong quá trình biến mất */
.slide-down-leave-active {
  transition: all 0.2s ease;
}

</style>