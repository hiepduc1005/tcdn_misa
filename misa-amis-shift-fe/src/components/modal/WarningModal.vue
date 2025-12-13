<script setup>
import BaseButton from '../button/BaseButton.vue';
import BaseModal from './BaseModal.vue';

const props = defineProps({
    showModal: {
        type: Boolean,
        default: false
    },
    title: {
        type: String,
        default: "Cảnh báo"
    },
    content: {
        type: String,
        default: ""
    },
    cancelLabel: {
        type: String,
        default: "Hủy"
    },
    width: {
        type: String,
        default: "width:432px;"
    }
})

const emit = defineEmits(['close'])

const handleClose = () => {
    emit('close');
}


</script>

<template>
    <BaseModal
        :showModal="props.showModal"
        :width="props.width"
    >
        <template #header>
            <div class="warning-header flex flex-row align-center between">
                <div class="warning-header-right flex flex-row align-center">
                    <!-- Icon có thể thay đổi tùy theo props.type nếu cần mở rộng sau này -->
                    <div class="icon20 icon-warning"></div>
                    <span>{{ props.title }}</span>
                </div>
                <div class="warning-header-left">
                    <div class="icon20 close-icon20 pointer" @click="handleClose"></div>
                </div>
            </div>
        </template>
        <template #body>
            <div class="warning-body">
                <!-- Sử dụng slot nếu muốn truyền HTML phức tạp, fallback là props.content -->
                <slot name="content">
                    <div v-html="props.content"></div>
                </slot>
            </div>
        </template>
        <template #footer>
            <div class="warning-footer flex flex-row flex-end">
                <BaseButton
                    type="outline-neutral"
                    @click="handleClose"
                >
                    <template #content>
                        <span>{{ props.cancelLabel }}</span>
                    </template>
                </BaseButton>
                
            </div>
        </template>
    </BaseModal>
</template>

<style scoped>
.warning-header{
    padding: 16px 16px 0 16px;
    margin-bottom: 16px;
}
.warning-header-right{
    font-weight: 600;
    color: #111827;
    font-size: 20px;
    gap: 8px; /* Khoảng cách giữa icon và text */
}

.warning-body{
    padding: 0 16px 0 16px;
    font-size: 13px;
    max-height: 400px;
    overflow-y: auto;
    font-weight: 400;
    line-height: 20px;
    max-width: 100%;
    overflow-wrap: anywhere;
    margin-bottom: 16px;
    color: #1f2937;
}

.warning-footer{
    padding: 0 16px 16px 16px;
    gap: 8px;
}
</style>