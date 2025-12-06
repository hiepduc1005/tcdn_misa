<script setup>
import { ref } from 'vue';
import BaseButton from '../components/button/BaseButton.vue';
import ShiftBody from './ShiftBody.vue';
import ShiftFormModal from './ShiftFormModal.vue';
import { SHIFT_MODAL_TYPE } from '../constants/common';

const datas = ref([
        {
            "ShiftCode": "1",
            "ShiftName": "12",
            "BeginShiftTime": "00:30:00",
            "EndShiftTime": "03:00:00",
            "BeginBreakTime": "00:30:00",
            "EndBreakTime": "01:00:00",
            "WorkingTime": 2.0000000000,
            "BreakingTime": 0.5000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-17T15:08:32.300+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-30T10:53:11.743+07:00",
            "ShiftID": "c69c345c-cf17-49bc-bc34-e7850118a7c5"
        },
        {
            "ShiftCode": "12345678901234567890123aa",
            "ShiftName": "12345678901234567890123456789012345678901234567890",
            "BeginShiftTime": "00:00:00",
            "EndShiftTime": "03:00:00",
            "BeginBreakTime": "00:30:00",
            "EndBreakTime": "01:00:00",
            "WorkingTime": 2.5000000000,
            "BreakingTime": 0.5000000000,
            "Inactive": true,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-20T14:35:45.111+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-30T22:09:46.160+07:00",
            "ShiftID": "216645ef-d6e9-4f47-b1f0-2641ec6c00ba"
        },
        {
            "ShiftCode": "a",
            "ShiftName": "s",
            "BeginShiftTime": "00:00:00",
            "EndShiftTime": "02:30:00",
            "BeginBreakTime": "00:30:00",
            "EndBreakTime": "02:00:00",
            "WorkingTime": 1.0000000000,
            "BreakingTime": 1.5000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-19T14:51:47.023+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-12-01T00:45:29.543+07:00",
            "ShiftID": "0d402e79-3630-4b02-90bf-61a20a5fefd4"
        },
        {
            "ShiftCode": "â",
            "ShiftName": "aaaaa",
            "BeginShiftTime": "00:30:00",
            "EndShiftTime": "01:30:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 1.0000000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-22T11:03:47.260+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-22T11:03:47.260+07:00",
            "ShiftID": "9288aee9-01c7-475a-a051-5fce44427608"
        },
        {
            "ShiftCode": "aaaa",
            "ShiftName": "aaa",
            "BeginShiftTime": "01:00:00",
            "EndShiftTime": "00:00:00",
            "BeginBreakTime": "01:30:00",
            "EndBreakTime": "01:00:00",
            "WorkingTime": -0.5000000000,
            "BreakingTime": 23.5000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-30T18:55:26.767+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-30T18:55:26.768+07:00",
            "ShiftID": "97a09e19-a993-4441-87c4-60707a4a8ed3"
        },
        {
            "ShiftCode": "aaaaaaaaaaa",
            "ShiftName": "aaa",
            "BeginShiftTime": "01:00:00",
            "EndShiftTime": "11:11:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 10.1833000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-30T18:57:45.690+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-30T18:57:45.692+07:00",
            "ShiftID": "66e90d84-cd25-4d5f-959c-27ae2221229e"
        },
        {
            "ShiftCode": "aaaaaaaavvvvv",
            "ShiftName": "ca 1",
            "BeginShiftTime": "00:00:00",
            "EndShiftTime": "12:30:00",
            "BeginBreakTime": "00:30:00",
            "EndBreakTime": "00:00:00",
            "WorkingTime": -11.0000000000,
            "BreakingTime": 23.5000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-22T18:57:41.333+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-29T16:57:22.135+07:00",
            "ShiftID": "731e678f-7778-4f42-a4d4-a58a62c406ed"
        },
        {
            "ShiftCode": "aaacccc",
            "ShiftName": "aaa",
            "BeginShiftTime": "00:00:00",
            "EndShiftTime": "11:11:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 11.1833000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-22T00:12:51.795+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-22T00:12:51.795+07:00",
            "ShiftID": "7f5e2578-ac33-44c8-b474-82dafb3adf00"
        },
        {
            "ShiftCode": "ac",
            "ShiftName": "âc",
            "BeginShiftTime": "00:00:00",
            "EndShiftTime": "23:30:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 23.5000000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-20T16:38:33.738+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-20T16:38:52.832+07:00",
            "ShiftID": "a266893a-19da-4455-a26c-c4d07f8abca1"
        },
        {
            "ShiftCode": "acvcvc",
            "ShiftName": "a",
            "BeginShiftTime": "01:30:00",
            "EndShiftTime": "14:00:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 12.5000000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-21T22:20:44.184+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-21T22:20:44.184+07:00",
            "ShiftID": "b2e8fac5-ac66-4ad0-ad2b-178a40be655f"
        },
        {
            "ShiftCode": "axxx",
            "ShiftName": "c",
            "BeginShiftTime": "00:30:00",
            "EndShiftTime": "11:11:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 10.6833000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-21T22:12:04.466+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-21T22:12:04.466+07:00",
            "ShiftID": "075fd3ed-63fb-4c36-9d3b-20490302025b"
        },
        {
            "ShiftCode": "haghga",
            "ShiftName": "hsgija",
            "BeginShiftTime": "00:00:00",
            "EndShiftTime": "11:33:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 11.5500000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-22T00:13:34.129+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-22T00:13:34.129+07:00",
            "ShiftID": "faeeb00f-99d4-49b8-871d-c60c5f980249"
        },
        {
            "ShiftCode": "mới2",
            "ShiftName": "ạh ouhfoha",
            "BeginShiftTime": "00:30:00",
            "EndShiftTime": "18:27:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 17.9500000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-22T00:13:58.683+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-22T00:45:04.774+07:00",
            "ShiftID": "81a9817b-5bfc-4067-a2fa-4d30c5849567"
        },
        {
            "ShiftCode": "w",
            "ShiftName": "wwwwa",
            "BeginShiftTime": "12:05:00",
            "EndShiftTime": "00:30:00",
            "BeginBreakTime": null,
            "EndBreakTime": null,
            "WorkingTime": 12.4167000000,
            "BreakingTime": 0.0000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-22T11:37:27.121+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-22T11:38:53.082+07:00",
            "ShiftID": "22bf6b2f-ceb6-492b-8f12-e1de364577e0"
        },
        {
            "ShiftCode": "xxxx",
            "ShiftName": "xxxx",
            "BeginShiftTime": "00:00:00",
            "EndShiftTime": "05:00:00",
            "BeginBreakTime": "00:30:00",
            "EndBreakTime": "02:00:00",
            "WorkingTime": 3.5000000000,
            "BreakingTime": 1.5000000000,
            "Inactive": false,
            "CreatedBy": "Đinh Nga QTHT",
            "CreatedDate": "2025-11-30T20:38:35.884+07:00",
            "ModifiedBy": "Đinh Nga QTHT",
            "ModifiedDate": "2025-11-30T20:38:35.884+07:00",
            "ShiftID": "9fd64887-bc3c-4933-9419-1e9fec52a2d5"
        }
    ]);

    const columns = [
        { 
            field: "ShiftCode", 
            label: "Mã ca", 
            filterable: true, 
        },
        { 
            field: "ShiftName", 
            label: "Tên ca", 
            filterable: true,
        },
        { 
            field: "Inactive", 
            label: "Trạng thái", 
            filterable: true, 
        },
        { 
            field: "BeginShiftTime", 
            label: "Giờ vào ca", 
            filterable: false, 
        },
        { 
            field: "EndShiftTime", 
            label: "Giờ hết ca", 
            filterable: false, 
        },
        { 
            field: "BeginBreakTime", 
            label: "Bắt đầu nghỉ giữa ca", 
            filterable: false, 
        },
        { 
            field: "EndBreakTime", 
            label: "Kết thúc nghỉ giữa ca", 
            filterable: false, 
        },
        { 
            field: "WorkingTime", 
            label: "Thời gian làm việc (giờ)", 
            filterable: true, 
        },
        { 
            field: "BreakingTime", 
            label: "Thời gian nghỉ giữa ca (giờ)", 
            filterable: true, 
        },
        { field: "CreatedBy", label: "Người tạo", filterable: true },
        { field: "CreatedDate", label: "Ngày tạo", filterable: true },
        { field: "ModifiedBy", label: "Người sửa", filterable: true },
        { field: "ModifiedDate", label: "Ngày sửa", filterable: true },
    ];

    const showFormModal = ref(false);

    const handleShowFormModal = () => {
        showFormModal.value = true;
    }

    const handleCloseModal = () => {
        showFormModal.value = false;
    }

    

</script>

<template>
    <div class="shifview flex flex-column content-center">
        <div class="shifview-header flex flex-row align-center between">
            <div class="shifview-header-title">Ca làm việc</div>
            <div class="shiftview-header-addbtn">
                <BaseButton
                    :type="'solid-brand'"
                    @click="handleShowFormModal()"
                >
                    <template #content>
                        <div class="icon16 icon-add-white"></div>
                        <span class="nowrap">Thêm</span>
                    </template>
                </BaseButton>
            </div>
        </div>
        <div class="shifview-body">
            <ShiftBody
                :datas="datas"
                :columns="columns"
            >

            </ShiftBody>
            <ShiftFormModal 
                :modalType="SHIFT_MODAL_TYPE.CREATE"
                :showModal="showFormModal"
                @close="handleCloseModal()"
            />
        </div>
    </div>
</template>

<style scoped>

.shifview{
    
    height: calc(100% - 50px);
}

.shifview-header{
    margin-bottom: 16px;
}

.shifview-header-title{
    font-size: 24px;
    font-weight: 700;
    color: #111827;
}

.shiftview-header-addbtn span{
    font-weight: 600;
    padding-left: .25rem;

}

.shifview-body{
    flex: 1;
    height: 0;
}

</style>