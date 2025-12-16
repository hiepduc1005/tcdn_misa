<script setup>
import { ref } from 'vue';
import accountAvatar from '../assets/images/account-avatar.png';
import BaseToolTip from '../components/tooltip/BaseToolTip.vue';
import { ElMessage } from 'element-plus';

const showDevelopingFeatureToast = () => {
  ElMessage({
    message: 'Tính năng đang trong quá trình phát triển',
    type: 'info',
    customClass: 'misa-toast',
    duration: 3000,
    showClose: true,
    offset: 32
  });
};
</script>

<template>
    <header  class="header">
        <div class="header__left flex flex-row">
            <div class="header__title flex flex-row gap12">
                <BaseToolTip
                    :content="'Tất cả ứng dụng'"
                
                >
                    <template #title>
                        <div class="icon24 logo-menu pointer"></div>
                    </template>
                </BaseToolTip>
                <div class="logo-production pointer"></div>
                <div class="title-name pointer">Sản xuất</div>
            </div>
            
        </div>
    
        <div class="header__right fullh flex flex-row">
            <div class="nav-item-content fullh flex flex-row align-center between fullw">
                <div class="nav-item-content-left flex flex-row align-center pointer">                  
                    <BaseToolTip
                       
                    
                    >
                        <template #content>
                            <span style="font-weight: 600;">COPENHAGEN DELIGHTS</span>
                        </template>
                        <template #title>
                            <span>COPENHAGEN DELIGHTS</span>
                        </template>
                    </BaseToolTip>
                    
                    <div class="icon16 angle-down-small"></div>
                </div>
                <div class="nav-item-content-right flex flex-row align-center">
                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Xuất excel</span>
                        </template>
                        <template #title>
                            <div class="icon24 export-excel-icon pointer" @click="showDevelopingFeatureToast"></div>
                        </template>
                    </BaseToolTip>

                    <div class="nav-separator"></div>

                    <BaseToolTip>
                        <template #content>
                            <span  style="font-weight: 600;">Tra cứu tồn kho</span>
                        </template>
                        <template #title>
                            <div class="icon24 package-icon pointer" @click="showDevelopingFeatureToast"></div>
                        </template>
                    </BaseToolTip>

                    <div class="invite-icon" @click="showDevelopingFeatureToast"></div>
                    
                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Thông báo</span>
                        </template>
                        <template #title>
                            <div class="icon24 notify-icon pointer" @click="showDevelopingFeatureToast"></div>
                        </template>
                    </BaseToolTip>

                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Trợ giúp</span>
                        </template>
                        <template #title>
                            <div class="icon24 question-icon pointer" @click="showDevelopingFeatureToast"></div>
                        </template>
                    </BaseToolTip>

                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Tính năng khác</span>
                        </template>
                        <template #title>
                            <div class="icon24 dot-circle-icon pointer" @click="showDevelopingFeatureToast"></div>
                        </template>
                    </BaseToolTip>

                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Kiến thức hữu ích</span>
                        </template>
                        <template #title>
                            <div class="knowledge-icon pointer" @click="showDevelopingFeatureToast"></div>
                        </template>
                    </BaseToolTip>

                    <div class="usercontrolpanel__account pointer">
                        <img :src="accountAvatar" alt="Avatar">
                    </div>
                </div>
            </div>
        </div>
    </header >
</template>

<style scoped>
    /* @import '@/assets/css/icon.css'; */
    .header{
        display: flex;
        justify-content: space-between;
        flex-shrink: 0;
        height: 48px;
        align-items: center;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        color: #1e2633;
        background: #ffffff;
        /* font-family: Inter; */
    }

    .header__left{
        max-width: 200px;
        display: flex;
        align-items: center;
        margin-right: 16px;
    }

    .header__left .header__title{
     
        width: 200px;
    }

    .header__right{
        flex: 1;
        padding: 0 16px 0 20px;
    }

    .header__right .nav-item-content-left span{
        max-width: 440px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        margin-right: 4px;
        font-weight: 600;
        font-size: 13px;
    }




    .header__title{
        padding-left: 16px;
        align-items: center;
    }

    .title-name{
        font-size: 18px;
        margin-left: 12px;
        font-weight: 700;
        color: #262626;
        white-space: nowrap;
    }
    
    .usercontrolpanel__account{
        margin-left: 16px;
    }
    .usercontrolpanel__account img{
        width: 100%;
        height: 100%;
        max-width: 32px;
        max-height: 32px;
        border-radius: 50%;
        object-fit: cover;
        
    }
</style>