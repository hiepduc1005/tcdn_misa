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
        default: ""
    },
    content: {
        type: String,
        default: ""
    },
    cancelLabel: {
        type: String,
        default: "Hủy"
    },
    confirmLabel:{
        type: String,
        default: "Đồng ý"
    },
    width: {
        type: String,
        default: "width:432px;"
    }
})

const emit = defineEmits(['close','confirm'])

const handleClose = () => {
    emit('close');
}

const handleConfirm = () => {
    emit('confirm');
}

</script>

<template>
    <BaseModal
        :showModal="props.showModal"
        :width="props.width"
    >
        <template #header>
            <div class="info-header flex flex-row align-center between">
                <div class="info-header-right flex flex-row align-center">
                    <!-- Icon có thể thay đổi tùy theo props.type nếu cần mở rộng sau này -->
                    <div class="icon20 infomation-icon"></div>
                    <span>{{ props.title }}</span>
                </div>
                <div class="info-header-left">
                    <div class="icon20 close-icon20 pointer" @click="handleClose"></div>
                </div>
            </div>
        </template>
        <template #body>
            <div class="info-body">
                <!-- Sử dụng slot nếu muốn truyền HTML phức tạp, fallback là props.content -->
                <slot name="content">
                    <div v-html="props.content"></div>
                </slot>
            </div>
        </template>
        <template #footer>
            <div class="info-footer flex flex-row flex-end">
                <BaseButton
                    type="outline-neutral"
                    @click="handleClose"
                >
                    <template #content>
                        <span>{{ props.cancelLabel }}</span>
                    </template>
                </BaseButton>
                <BaseButton
                    type="solid-brand"
                    @click="handleConfirm"
                >
                    <template #content>
                        <span>{{ props.confirmLabel }}</span>
                    </template>
                </BaseButton>
            </div>
        </template>
    </BaseModal>
</template>

<style scoped>
.info-header{
    padding: 16px 16px 0 16px;
    margin-bottom: 16px;
}
.info-header-right{
    font-weight: 600;
    color: #111827;
    font-size: 20px;
    gap: 8px; /* Khoảng cách giữa icon và text */
}

.info-body{
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

.info-footer{
    padding: 0 16px 16px 16px;
    gap: 8px;
}
</style>