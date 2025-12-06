<script setup>
import { nextTick, onMounted, ref, watch } from 'vue';

const props = defineProps({
    parentEl: { type: HTMLElement, required: true },
    show: { type: Boolean, default: false }
});

const emit = defineEmits(['close']);
const popoverRef = ref(null);

const updatePosition = () => {
    if (!props.parentEl || !popoverRef.value) return;
    const rect = props.parentEl.getBoundingClientRect();
    const popover = popoverRef.value;

    const fullW = window.innerWidth;
    const popoverRight = fullW - rect.right;
    const popoverTop = rect.bottom;

    popover.style.right = popoverRight + "px";
    popover.style.top = popoverTop + "px";

}

onMounted(() => {
    if (props.show) updatePosition();
})

watch(() => props.show, async (val) => {
    if(val){

        await nextTick();
        updatePosition();
    }
})

</script>

<template >
    <Teleport to="body">
        <transition name="dropdown">
            <div v-if="show" class="overlay" @click="emit('close')">
                <div ref="popoverRef" class="base-popover-container" @click.stop>
                    <slot name="content"></slot>
                </div>
            </div>
        </transition>
    </Teleport>
</template>

<style scoped>
.overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background: transparent; 
    z-index: 999; 
}

.base-popover-container{
    position: fixed; /* cố định theo viewport */
    z-index: 1000;
    background: white;
    overflow: auto;
    min-width: 0px;
    padding: 8px 0px;
    box-shadow: 0 0 8px #0000001a, 0 8px 16px #0000001a;
    border-radius: 4px;
    margin: 0;
}



/* Animation */
.dropdown-enter-from {
  opacity: 0;
  transform: translateY(-10px);
}
.dropdown-enter-active {
  transition: all 0.2s ease-out;
}
.dropdown-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
.dropdown-leave-active {
  transition: all 0.2s ease-in;
}
</style>