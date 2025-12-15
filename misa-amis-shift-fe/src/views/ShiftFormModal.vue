<script setup>
import { reactive, watch, ref, computed, onMounted, onUnmounted, nextTick } from 'vue';
import BaseButton from '../components/button/BaseButton.vue';
import BaseInputDate from '../components/input/BaseInputDate.vue';
import BaseInputText from '../components/input/BaseInputText.vue';
import BaseTextArea from '../components/input/BaseTextArea.vue';
import BaseTimePicker from '../components/input/BaseTimePicker.vue';
import BaseModal from '../components/modal/BaseModal.vue';
import BaseToolTip from '../components/tooltip/BaseToolTip.vue';
import WarningModal from '../components/modal/WarningModal.vue';
import BaseRadio from '../components/input/BaseRadio.vue';
import { SHIFT_MODAL_TYPE } from '../constants/common';
import ShiftAPI from '../apis/components/shift/ShiftAPI';
import { roundNumber, formatTimeToHHMM } from '../utils/formatFns';
import InfoModal from '../components/modal/InfoModal.vue';
import { isEqual } from 'lodash';

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
    handleShowFormModal:{
        type: Function,
        default: () => {}
    }
})

const emit = defineEmits(['close', 'success', 'saveAndAdd'])

const formState = reactive({
    shiftCode: '',
    shiftName: '',
    beginShiftTime: '',
    endShiftTime: '',
    beginBreakTime: '',
    endBreakTime: '',
    workingTime: 0,
    breakingTime: 0,
    inactive: false,
    description: ''
});

const showWarning = ref(false);
const warningMessage = ref("");

// Mở modal info để nhắc về dữ liệu đã thay đổi
const showInfo = ref(false);

// Biến theo dõi form đã bị thay đổi chưa
const isChanged = ref(false);

//Clone form để so sánh với form gốc
const cloneForm = ref({ ...formState });

//Theo dõi form nếu thay đổi thì so sánh luôn
watch(formState,(newVal) => {
    // Nếu như đã thay đổi rồi thì thôi không cần so sanh nữa 
    if(!isChanged.value){
        isChanged.value = !isEqual(newVal, cloneForm.value);
    }
},{deep:true})

// Để lưu trạng thái lỗi của từng input, giúp hiển thị viền đỏ và tooltip
const formErrors = reactive({
    shiftCode: false,
    shiftName: false,
    beginShiftTime: false,
    endShiftTime: false,
});


// Helper chuyển HH:mm sang phút
const timeToMinutes = (timeStr) => {
    if (!timeStr) return -1;
    const [h, m] = timeStr.split(':').map(Number);
    return h * 60 + m;
}

// Helper tính khoảng thời gian (giờ)
const calculateDiffHours = (start, end) => {
    if (!start || !end) return 0;
    
    const [h1, m1] = start.split(':').map(Number);
    const [h2, m2] = end.split(':').map(Number);
    
    let minutes1 = h1 * 60 + m1;
    let minutes2 = h2 * 60 + m2;

    let diff = minutes2 - minutes1;

    // Nếu diff bé hơn 0 thì có nghĩa là qua ngày hôm sau
    if (diff < 0) diff += 24 * 60; // Xử lý qua đêm
    
    return parseFloat((diff / 60).toFixed(10)); // Trả về số thực
}

// Watch để tự động tính toán
watch(
    () => [formState.beginBreakTime, formState.endBreakTime],
    ([start, end]) => {
        if (start && end) {
            formState.breakingTime = calculateDiffHours(start, end);
        } else {
            formState.breakingTime = 0;
        }
    }
);

// Tự động tắt lỗi khi người dùng nhập vào input đó
watch(formState, (newVal) => {
    if (newVal.shiftCode) formErrors.shiftCode = false;
    if (newVal.shiftName) formErrors.shiftName = false;
    if (newVal.beginShiftTime) formErrors.beginShiftTime = false;
    if (newVal.endShiftTime) formErrors.endShiftTime = false;
}, { deep: true });

watch(
    () => [formState.beginShiftTime, formState.endShiftTime, formState.breakingTime],
    ([start, end]) => {
        if (start && end) {
            formState.workingTime = calculateDiffHours(start, end) - formState.breakingTime;
        } else {
            formState.workingTime = 0;
        }
    }
);

/**
 * Định dạng số giờ làm việc để hiện thị lên form
 *
 * @author hiepnd
 * @returns {number} Số giờ sau khi được làm tròn.
 */
const workingTimeDisplay = computed(() => {
    if(formState.workingTime === 0) return 0;
    const roundedValue = roundNumber(formState.workingTime);
    if (roundedValue < 0) {
        return `(${Math.abs(roundedValue)})`;
    }
    return roundedValue;
})

/**
 * Định dạng số giờ nghỉ để hiện thị lên form
 *
 * @author hiepnd
 * @returns {number} Số giờ sau khi được làm tròn.
 */
const breakingTimeDisplay = computed(() => {
    if(formState.breakingTime === 0) return 0;

    return roundNumber(formState.breakingTime)
})

/**
 * Màu đỏ nếu như giờ làm việc âm
 *
 * @author hiepnd
 */
const workingTimeInputStyle = computed(() => {
    let baseStyle = '';
    if (formState.workingTime < 0) {
        baseStyle = 'color: red;';
    }
    return baseStyle;
});


// Reset form khi đóng/mở modal
watch(() => props.showModal, (val) => {
    if (val) {
        if (props.modalType === SHIFT_MODAL_TYPE.CREATE) {
            // Kiểm tra nếu có data truyền vào (Trường hợp Nhân bản)
            if (props.data && Object.keys(props.data).length > 0) {
                Object.assign(formState, {
                    ...props.data,
                    shiftCode: '', // Đảm bảo mã ca trống
                    shiftName: props.data.shiftName || '',
                    beginShiftTime: formatTimeToHHMM(props.data.beginShiftTime) || '',
                    endShiftTime: formatTimeToHHMM(props.data.endShiftTime) || '',
                    beginBreakTime: formatTimeToHHMM(props.data.beginBreakTime) || '',
                    endBreakTime: formatTimeToHHMM(props.data.endBreakTime) || '',
                    description: props.data.description || '',
                    inactive: false // Mặc định active khi nhân bản
                });
            } else {
                // Trường hợp Thêm mới thông thường
                Object.assign(formState, {
                    shiftCode: '',
                    shiftName: '',
                    beginShiftTime: '',
                    endShiftTime: '',
                    beginBreakTime: '',
                    endBreakTime: '',
                    workingTime: 0,
                    breakingTime: 0,
                    inactive: false,
                    description: ''
                });
            }
        } else {
            // Update mode: Map props.data to formState
            Object.assign(formState, {
                ...props.data,
                // Đảm bảo các trường thời gian không bị null/undefined và đúng định dạng
                beginShiftTime: formatTimeToHHMM(props.data.beginShiftTime) || '',
                endShiftTime: formatTimeToHHMM(props.data.endShiftTime) || '',
                beginBreakTime: formatTimeToHHMM(props.data.beginBreakTime) || '',
                endBreakTime: formatTimeToHHMM(props.data.endBreakTime) || '',
                description: props.data.description || ''
            });

        }
        
        // Reset lỗi khi mở modal
        Object.keys(formErrors).forEach(key => formErrors[key] = false);

        // Reset lại clone khi mở modal sau khi DOM và các watcher khác đã cập nhật
        nextTick(() => {
            cloneForm.value = JSON.parse(JSON.stringify(formState));
            isChanged.value = false;
        });
    }
});

/**
 * Validate số giờ nghỉ dựa trên giờ bắt đầu nghỉ và kết thúc nghỉ
 *
 * @author hiepnd
 */
const validateBreakTime = () => {
    // Nếu không nhập giờ nghỉ thì không cần check
    if (!formState.beginBreakTime) return true;

    const start = timeToMinutes(formState.beginShiftTime);
    const end = timeToMinutes(formState.endShiftTime);
    const breakStart = timeToMinutes(formState.beginBreakTime);
    
    // Chưa nhập đủ giờ vào/ra ca
    if (start === -1 || end === -1) return true; // Sẽ được bắt bởi validateForm()

    // Trường hợp ca thường (ví dụ: 08:00 -> 17:00)
    if (start < end) {
        if (breakStart < start || breakStart > end) {
            return false;
        }
    } 
    // Trường hợp qua đêm (ví dụ: 22:00 -> 06:00)
    else {
        // Break phải nằm trong [Start -> 23:59] HOẶC [00:00 -> End]
        // Tức là KHÔNG ĐƯỢC nằm trong khoảng [End -> Start] (khoảng nghỉ giữa 2 ca)
        if (breakStart > end && breakStart < start) {
            return false;
        }
    }

    return true;
}

/**
 * Hàm kiểm tra tất cả các trường bắt buộc

 * @author hiepnd
 */
const validateForm = () => {
    let isValid = true;
    let firstErrorMessage = ""; // Chỉ lưu thông báo lỗi đầu tiên

    // Reset lỗi trước khi kiểm tra
    Object.keys(formErrors).forEach(key => formErrors[key] = false);

    // Kiểm tra từng trường, nếu lỗi thì đánh dấu và lưu message (nếu chưa có message nào)
    if (!formState.shiftCode) {
        formErrors.shiftCode = true;
        if (!firstErrorMessage) firstErrorMessage = "Mã ca không được để trống.";
        isValid = false;
    }
    if (!formState.shiftName) {
        formErrors.shiftName = true;
        if (!firstErrorMessage) firstErrorMessage = "Tên ca không được để trống.";
        isValid = false;
    }
    if (!formState.beginShiftTime) {
        formErrors.beginShiftTime = true;
        if (!firstErrorMessage) firstErrorMessage = "Giờ vào ca không được để trống.";
        isValid = false;
    }
    if (!formState.endShiftTime) {
        formErrors.endShiftTime = true;
        if (!firstErrorMessage) firstErrorMessage = "Giờ hết ca không được để trống.";
        isValid = false;
    }

    if (!isValid) {
        warningMessage.value = firstErrorMessage; // Chỉ hiển thị lỗi đầu tiên
    }
    return isValid;
};

/**
 * Hàm dùng để lưu ca làm việc
 *
 * @author hiepnd
 */
const handleSave = async () => {
    try {
        // 1. Kiểm tra các trường bắt buộc trước
        if (!validateForm()) {
            showWarning.value = true;
            return;
        }

        // 2. Validation Logic Giờ nghỉ
        if (!validateBreakTime()) {
            warningMessage.value = "Thời gian bắt đầu nghỉ giữa ca phải nằm trong khoảng thời gian tính từ giờ vào ca đến giờ hết ca. Vui lòng kiểm tra lại.";
            showWarning.value = true;
            return;
        }

        try{
            if (props.modalType === SHIFT_MODAL_TYPE.UPDATE) {
                const res = await ShiftAPI.update(formState);
                
            } else {
                await ShiftAPI.insert(formState);
            }

            emit('success'); // Báo cho cha biết để reload data
            
            // Cập nhật lại trạng thái gốc sau khi lưu thành công để tránh cảnh báo khi đóng
            cloneForm.value = { ...formState };
            isChanged.value = false;

            handleCloseModal();
        }catch(err){
            if(err.response.status === 400){
                showWarning.value = true;
                const errors = err.response.data.Errors;
                const firstMessage = Object.values(errors)[0];  // Lấy message đầu tiên
                warningMessage.value = firstMessage
            }
        }

        
    } catch (error) {
        console.error("Lỗi thêm mới:", error);
    }

}

/**
 * Hàm dùng để lưu ca làm việc và mở modal thêm ca làm việc mới 
 *
 * @author hiepnd
 */
const handleSaveAndCreate = async () => {
    await handleSave();
    // handleclo
    props.handleShowFormModal()
}

/**
 * Hàm dùng để đóng modal
 *
 * @author hiepnd
 */
const handleCloseModal = () => {
    // Nếu như form đã thay dổi thì mở modal cảnh báo người dùng
    if(isChanged.value){
        showInfo.value = true
    }
    // Nếu k thì đóng modal
    else {

        emit('close');
    }
}

// Nếu như người dùng đồng ý đóng modal 
const handleConfirmClose = () => {
    emit('close');
    isChanged.value = false
    showInfo.value = false
}

// Hàm để nhấn phím tắt cho modal
const handleKeydown = async (e) => {
    if (!props.showModal) return; // Chỉ xử lý khi modal đang mở

    if (e.key === 'Escape') {
        e.preventDefault(); // Ngăn hành vi mặc định của trình duyệt
        handleCloseModal();
    } else if (e.ctrlKey && e.shiftKey && e.key === 'S') { // Ctrl + Shift + S
        e.preventDefault(); 
        await handleSaveAndCreate();
    } else if (e.ctrlKey && e.key === 's') { // Ctrl + S
        e.preventDefault(); 
        await handleSave();
    }
}

/**
 * Thêm sự kiện cho bàn phím khi component mounted
 *
 * @author hiepnd
 */
onMounted(() => {
    window.addEventListener('keydown', handleKeydown);
})

/**
 * Xóa sự kiện bàn phím khi component unmounted để tránh memory leak
 *
 * @author hiepnd
 */
onUnmounted(() => {
    window.removeEventListener('keydown', handleKeydown);
})

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
                                v-model="formState.shiftCode"
                                :required="true"
                                :isError="formErrors.shiftCode"
                                errorMessage="Mã ca không được để trống"
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
                                v-model="formState.shiftName"
                                :required="true"
                                :isError="formErrors.shiftName"
                                errorMessage="Tên ca không được để trống"
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
                                v-model="formState.beginShiftTime"
                                :required="true"
                                :isError="formErrors.beginShiftTime"
                                errorMessage="Giờ vào ca không được để trống"
                            />
                        </div>
                    </div>
                    <div class="flex flex-row align-center gap-4 fullw between">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Giờ hết ca</label>
                            <div class="field-required">&nbsp;*</div>
                        </div>
                            <BaseTimePicker 
                                v-model="formState.endShiftTime"
                                :required="true"
                                :isError="formErrors.endShiftTime"
                                errorMessage="Giờ hết ca không được để trống"
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
                                v-model="formState.beginBreakTime"
                            />
                        </div>
                    </div>
                    <div class="flex flex-row align-center gap-4 fullw between">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Kết thúc nghỉ giữa ca</label>
                        </div>
                            <BaseTimePicker 
                                v-model="formState.endBreakTime"
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
                                :style="'max-height:28px;min-height:28px; max-width:122px;'"
                                :disabled="true"
                                v-model="workingTimeDisplay"
                                :inputColor="workingTimeInputStyle"
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
                                v-model="breakingTimeDisplay"
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
                                v-model="formState.description"
                            />
                        </div>
                    </div>
                </div>
                <div v-if="props.modalType === SHIFT_MODAL_TYPE.UPDATE" class="form-group">
                    <div class="flex flex-row items-start gap-4 fullw">
                        <div style="width: 150px;" class="flex flex-row align-center">
                            <label class="label">Trạng thái</label>
                        </div>
                        <div class="flex align-center pointer flex-1">
                            <BaseRadio
                                :value="false"
                                v-model="formState.inactive"
                                inputName="inactive"
                            >
                                Đang sử dụng
                            </BaseRadio>
                            <BaseRadio
                                :value="true"
                                v-model="formState.inactive"
                                inputName="inactive"
                            >
                                Ngừng sử dụng
                            </BaseRadio>
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
                            @click="handleCloseModal"
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
                                    @click="handleSaveAndCreate"
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
                                    @click="handleSave"
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

    <WarningModal 
        :showModal="showWarning"
        :title="'Cảnh báo'"
        :content="warningMessage"
        :cancelLabel="'Đóng'"
        @close="showWarning = false"
    >
        <!-- Có thể bỏ nút confirm nếu chỉ là cảnh báo -->
        <template #footer>
             <div class="warning-footer flex flex-row flex-end">
                <BaseButton
                    type="solid-brand"
                    @click="showWarning = false"
                >
                    <template #content>
                        <span>Đóng</span>
                    </template>
                </BaseButton>
            </div>
        </template>
    </WarningModal>
    <InfoModal
        :title="'Thoát và không lưu?'"
        :content="'Nếu bạn thoát, các dữ liệu đang nhập liệu sẽ không được lưu lại.'"
        @close="() => showInfo = false"
        @confirm="() => handleConfirmClose()"
        :showModal="showInfo"
    >

    </InfoModal>
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

/* Warning Modal Footer Style override if needed */
.warning-footer{
    padding: 0 16px 16px 16px;
    gap: 8px;
}

</style>