<script setup>
import { nextTick, onMounted, ref, watch } from 'vue';
import { TEXT_FILTER_OPERATORS } from '../../constants/common';
import BaseButton from '../button/BaseButton.vue';
import BaseInputText from '../input/BaseInputText.vue';
import BaseSelectInput from '../input/BaseSelectInput.vue';


const emit = defineEmits(['close']);

const props = defineProps({
    title: {
        type: String,
        default: ''
    },
    parentEl: {
        type: HTMLElement,
        required: true
    },
    showPopup: {
        type: Boolean,
        default: false
    },
    type: {
        type: String,
        default: 'text'   /* text / date / number */
    }

})

const popupRef = ref(null);


/**
 * Cập nhật vị trí hiển thị của popup dựa theo vị trí của phần tử cha (parentEl).
 *
 * Logic:
 * - Lấy bounding rect của phần tử cha.
 * - Lấy kích thước popup.
 * - Nếu popup bị tràn ra ngoài màn hình bên phải,
 *   → canh theo `right`.
 * - Ngược lại,
 *   → canh theo `left`.
 * - Luôn đặt popup ngay bên dưới parent (top = rect.bottom).
 *
 * @author hiepnd
 * @since 2025-12-05
 * @date 2025-12-05
 * @returns {void}
 */

const updatePosition = () => {
    if (!props.parentEl || !popupRef.value) return;
    const rect = props.parentEl.getBoundingClientRect();
    const popup = popupRef.value;

    if(window.innerWidth < rect.left + popup.getBoundingClientRect().width){
        popup.style.top = rect.bottom + "px";
        popup.style.right = window.innerWidth - rect.right + "px";
    }else {
        popup.style.top = rect.bottom + "px";
        popup.style.left = rect.left + "px";
    }

};

onMounted(() => {
    if (props.showPopup) updatePosition();
});

watch(() => props.showPopup,async  (val) => {
    if (val) {
        await nextTick(); 
        updatePosition();
    }
});

</script>

<template>
    <Teleport to="body">
        <div class="overlay" v-show="showPopup" @click="emit('close')">
            <div ref="popupRef"  class="condition-container flex flex-column content-center" @click.stop>
                <div class="condition-header flex flex-row align-center between">
                    <slot name="header">
                        <div class="title">Lọc {{ props.title }}</div>
                        <div class="close-condition-btn">
                            <BaseButton
                                :type="'text-neutral'"  
                                @click="emit('close')"
                            >
                                <template #content>
                                    <div class="icon16 close-icon"></div>
                                </template>
                            </BaseButton>
                        </div>
                    </slot>
                </div>
                <div class="filter-container flex flex-column">
                    <slot name="body">
                        <BaseSelectInput
                            :options="TEXT_FILTER_OPERATORS"
                            :filterable="true"
                            :default-first-option="true"
                            :defaultValue="'contains'"
                        >
                        </BaseSelectInput>
    
                        <BaseInputText
                            :type="'text'"
                            :placeholder="'Nhập giá trị lọc'"
                        >
                        
                        </BaseInputText>
                    </slot>
                </div>
    
                <div class="condition-buttons flex flex-row align-center between">
                    <slot name="footer">
                        <div class="condition-buttons-left">
                            <BaseButton
                                :type="'filled-neutral'"  
                            >
                                <template #content>
                                    <span>Bỏ lọc</span>
                                </template>
                            </BaseButton>
                        </div>
                        <div class="condition-buttons-right flex flex-row align-center">
                            <BaseButton
                                :type="'outline-neutral'"  
                                @click="emit('close')"
                            >
                                <template #content>
                                    <span>Hủy</span>
                                </template>
                            </BaseButton>
    
                            <BaseButton
                                :type="'solid-brand'"  
                            >
                                <template #content>
                                    <span>Áp dụng</span>
                                </template>
                            </BaseButton>
                        </div>
                    </slot>
                </div>
            </div>
        </div>
    </Teleport>
</template>

<style scoped>

.overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    z-index: 999;
    background: transparent;
}

.condition-container{
    position: fixed;  /* cố định theo viewport */

    z-index: 1000;
    background: white;
    min-width: 350px;
    width: -moz-fit-content;
    width: fit-content;
    font-weight: 400;
    font-size: 13px;
    border-radius: 4px;
    box-shadow: 0 0 8px #0000001a, 0 8px 16px #0000001a;
    padding: 16px;
    gap: 16px;
}

.condition-container .title{
    font-weight: 600;
    font-size: 16px;
    margin-right: 30px;
}

.filter-container{
    text-align: left;
    gap: 8px;
}

.condition-buttons-right{
    gap: 8px;
}

.overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background: transparent; 
    z-index: 999; 
}


</style>