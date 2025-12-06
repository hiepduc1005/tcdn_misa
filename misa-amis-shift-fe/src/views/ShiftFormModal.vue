<script setup>
import BaseButton from '../components/button/BaseButton.vue';
import BaseInputDate from '../components/input/BaseInputDate.vue';
import BaseInputText from '../components/input/BaseInputText.vue';
import BaseTextArea from '../components/input/BaseTextArea.vue';
import BaseTimePicker from '../components/input/BaseTimePicker.vue';
import BaseModal from '../components/modal/BaseModal.vue';
import BaseToolTip from '../components/tooltip/BaseToolTip.vue';
import { SHIFT_MODAL_TYPE } from '../constants/common';

const props = defineProps({
    width: {
        type: String,
        default: "width:680px;"
    },
    showModal: {
        type: Boolean,
        default: false
    },
    data:{
        type: Object,
        default: () =>{}
    },
    modalType: {
        type: String,
        default: SHIFT_MODAL_TYPE.CREATE
    },
})

const emit = defineEmits(['close'])

const handleCloseModal = () => {
    emit('close');
}

</script>

<template>
    <BaseModal
        :showModal="showModal"
        :width="props.width"
    >
        <template #header>
            <div class="modal-title flex flex-row align-center between">
                <div class="title-left">
                    <div class="title">{{ props.modalType === SHIFT_MODAL_TYPE.CREATE ? "Thêm" : "Sửa" }} Ca làm việc</div>
                </div>
                <div class="title-right flex flex-row align-center">
                    <BaseToolTip
                        :placement="'top'"                                       
                    >
                        <template #title>
                            <div class="feature-btn">
                                <div class="icon20 help-icon pointer"></div>
                            </div>
                        </template>

                        <template #content>
                            <span>Trợ giúp</span>
                        </template>
                    </BaseToolTip>

                    <BaseToolTip
                        :placement="'top'"                                       
                    >
                        <template #title>
                            <div class="feature-btn" @click="handleCloseModal()">
                                <div class="icon20 close-icon20 pointer"></div>
                            </div>
                        </template>

                        <template #content>
                            <span>Đóng (ESC)</span>
                        </template>
                    </BaseToolTip>
                </div>
            </div>
        </template>

        <template #body>
            <div class="container-content">
                <div class="form-group">
                    <div class="flex flex-row align-center gap-4 fullw">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Mã ca</label>
                            <div class="field-required">&nbsp;*</div>
                        </div>
                        <div class="flex align-center pointer flex-1">
                            <BaseInputText 
                                :style="'max-height:28px;min-height:28px;'"
                            />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="flex flex-row align-center gap-4 fullw">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Tên ca</label>
                            <div class="field-required">&nbsp;*</div>
                        </div>
                        <div class="flex align-center pointer flex-1">
                            <BaseInputText 
                                :style="'max-height:28px;min-height:28px;'"
                            />
                        </div>
                    </div>
                </div>
                <div class="form-group flex flex-row align-center" >
                    <div class="flex flex-row align-center gap-4 fullw" style="margin-right: 12px;">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Giờ vào ca</label>
                            <div class="field-required">&nbsp;*</div>
                        </div>
                        <div class="flex align-center pointer flex-1">
                            <BaseTimePicker 
                                
                            />
                        </div>
                    </div>
                    <div class="flex flex-row align-center gap-4 fullw between">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Giờ hết ca</label>
                            <div class="field-required">&nbsp;*</div>
                        </div>
                            <BaseTimePicker 
                            />
                    </div>
                </div>
                <div class="form-group flex flex-row align-center" >
                    <div class="flex flex-row align-center gap-4 fullw" style="margin-right: 12px;">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Bắt đầu nghỉ giữa ca</label>
                        </div>
                        <div class="flex align-center pointer flex-1">
                            <BaseTimePicker 
                                
                            />
                        </div>
                    </div>
                    <div class="flex flex-row align-center gap-4 fullw between">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Kết thúc nghỉ giữa ca</label>
                        </div>
                            <BaseTimePicker 
                            />
                    </div>
                </div>
                <div class="form-group flex flex-row align-center" >
                    <div class="flex flex-row align-center gap-4 fullw" style="margin-right: 12px;">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Thời gian làm việc (giờ)</label>
                        </div>
                        <div class="flex align-center pointer flex-1">
                            <BaseInputText 
                                :style="'max-height:28px;min-height:28px;max-width:122px;'"
                                :disabled="true"
                            />
                        </div>
                    </div>
                    <div class="flex flex-row align-center gap-4 fullw between">
                        <div style="width: auto;" class="flex flex-row align-center">
                            <label class="label">Thời gian nghỉ giữa ca (giờ)</label>
                        </div>
                            <BaseInputText 
                                :style="'max-height:28px;min-height:28px; max-width:122px;'"
                                :disabled="true"
                            />
                    </div>
                </div>
                <div class="form-group">
                    <div class="flex flex-row items-start gap-4 fullw">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Mô tả</label>
                        </div>
                        <div class="flex align-center pointer flex-1">
                            <BaseTextArea 
                                
                            />
                        </div>
                    </div>
                </div>
            </div>
        </template>

        <template #footer>
            <div class="modal__footer__line"></div>
            <div class="modal__footer">
                <div class="footer-buttons flex flex-row flex-end fullw gap-4">
                    <div class="cancel-button">
                        <BaseButton
                            type="outline-neutral"
                        >
                            <template #content>
                                <span>Hủy</span>
                            </template>
                        </BaseButton>
                    </div>
                    <div class="save-and-add-btn">
                        <BaseToolTip
                            :placement="'top'"
                        >
                            <template #title>
                                <BaseButton
                                    type="outline-neutral"
                                >
                                    <template #content>
                                        <span>Lưu và Thêm</span>
                                    </template>
                                </BaseButton>
                            </template>

                            <template #content>
                                <span>Ctrl + Shift + S</span>
                            </template>
                        </BaseToolTip>
                    </div>
                    <div class="close-button">
                        <BaseToolTip
                            :placement="'top'"
                        >
                            <template #title>
                                <BaseButton
                                    type="solid-brand"
                                >
                                    <template #content>
                                        <span>Lưu</span>
                                    </template>
                                </BaseButton>
                            </template>

                            <template #content>
                                <span>Ctrl + S</span>
                            </template>
                        </BaseToolTip>
                    </div>               
                </div>
            </div>
        </template>
    </BaseModal>
</template>

<style scoped>

.modal-title{
    padding: 16px 20px;
}

.modal-title .title-left{
    flex: 1 1 0%;
    min-width: 0;
    align-items: center;
    font-weight: 700;
    font-size: 16px;
    line-height: 36px;
    text-overflow: ellipsis;
    overflow: hidden;
    display: flex;
}

.title-left .title{
    font-size: 24px;
    color: #111827;
    margin-right: 32px;
    white-space: nowrap;
    cursor: text;
}

.modal-title .title-right{
    column-gap: 8px;
    justify-content: flex-end;
    white-space: nowrap;
    flex-shrink: 0;
}

.container-content{
    padding: 20px;
}

.modal__footer__line{
    border-top: 1px solid #d5dfe2;
}

.modal__footer{
    display: flex;
    padding: 12px 20px;
    position: relative;
}

</style>