<script setup>
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import { COLUMN_TYPE, NUMBER_DATE_FILTER_OPERATORS, TEXT_FILTER_OPERATORS } from '../../constants/common';
import BaseButton from '../button/BaseButton.vue';
import BaseInputText from '../input/BaseInputText.vue';
import BaseSelectInput from '../input/BaseSelectInput.vue';
import BaseInputDate from '../input/BaseInputDate.vue';

// Emit
const emit = defineEmits(['close','change','clear','apply-filter']);

// Props
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
        default: COLUMN_TYPE.TEXT   /* text / date / number */
    },
    options: {
        type: Array,
    },
    field : {
        type: String,
        default: ''
    },
    currentFilter:{
        type: Object,
        default: null
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
        // Cập nhật giá trị từ currentFilter khi mở popup
        if (props.currentFilter) {
            operator.value = props.currentFilter.operator;
            value.value = props.currentFilter.value;
        } else {
            // Reset về mặc định nếu không có filter hiện tại
            value.value = '';
        }

        await nextTick(); 
        updatePosition();
    }
});

const operator = ref(props.options?.[0]?.value ?? '');

const value = ref('')

const handleChangeOperator = (val) => {
    operator.value = val
}

// Nếu như là "Trống" hoặc "Không trống" thì không cần nhập
const isInputDisabled = computed(() => {
    if(operator.value === 'empty' || operator.value === 'not_empty'){
        value.value = '';
        return true;
    }
    return false;
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
                            :options="props.options"
                            :filterable="true"
                            :default-first-option="true"
                            :defaultValue="props.options[0].value"
                            @select="handleChangeOperator"
                        >
                        </BaseSelectInput>
    
                        <!-- Nếu như type là select thì chỉ cần hiển thị select k cần input -->
                        <BaseInputText
                            v-if="props.type !== COLUMN_TYPE.DATE && props.type !== COLUMN_TYPE.SELECT"
                            :type="props.type"
                            :placeholder="'Nhập giá trị lọc'"
                            v-model="value"
                            :disabled="isInputDisabled"
                        >
                        </BaseInputText>

                        <!-- Nếu như là select thì chỉ cần hiển thị select k cần input -->
                        <BaseInputDate
                            v-if="props.type === COLUMN_TYPE.DATE && props.type !== COLUMN_TYPE.SELECT"
                            placeholder="Nhập giá trị lọc"
                            v-model="value"
                        >
                        </BaseInputDate>
                    </slot>
                </div>
    
                <div class="condition-buttons flex flex-row align-center between">
                    <slot name="footer">
                        <div class="condition-buttons-left">
                            <BaseButton
                                :type="'filled-neutral'"  
                                @click="emit('clear',props.field)"
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
                                @click="emit('apply-filter', {
                                    field: props.field,
                                    operator: operator,
                                    value: value
                                })"  
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