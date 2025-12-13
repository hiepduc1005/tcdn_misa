<script setup>
import { ref } from 'vue';
import accountAvatar from '../assets/images/account-avatar.png';
import BaseToolTip from '../components/tooltip/BaseToolTip.vue';

const isInviteBoxOpen = ref(false);

const handleOpenInviteBox = () => {
    isInviteBoxOpen.value = true;
}

const handleCloseInviteBox = () => {
    isInviteBoxOpen.value = false;
}

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
                            <div class="icon24 export-excel-icon pointer"></div>
                        </template>
                    </BaseToolTip>

                    <div class="nav-separator"></div>

                    <BaseToolTip>
                        <template #content>
                            <span  style="font-weight: 600;">Tra cứu tồn kho</span>
                        </template>
                        <template #title>
                            <div class="icon24 package-icon pointer"></div>
                        </template>
                    </BaseToolTip>

                    <div @mouseenter="handleOpenInviteBox" @mouseleave="handleCloseInviteBox" class="invite-icon"></div>
                    
                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Thông báo</span>
                        </template>
                        <template #title>
                            <div class="icon24 notify-icon pointer"></div>
                        </template>
                    </BaseToolTip>

                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Trợ giúp</span>
                        </template>
                        <template #title>
                            <div class="icon24 question-icon pointer"></div>
                        </template>
                    </BaseToolTip>

                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Tính năng khác</span>
                        </template>
                        <template #title>
                            <div class="icon24 dot-circle-icon pointer"></div>
                        </template>
                    </BaseToolTip>

                    <BaseToolTip>
                        <template #content>
                            <span style="font-weight: 600;">Kiến thức hữu ích</span>
                        </template>
                        <template #title>
                            <div class="knowledge-icon pointer"></div>
                        </template>
                    </BaseToolTip>

                    <div class="usercontrolpanel__account pointer">
                        <img :src="accountAvatar" alt="Avatar">
                    </div>
                </div>
            </div>
        </div>

        <div class="invite-box-popup" v-show="isInviteBoxOpen">
            <div class="invite-box-header">
                <div class="invite-box-title">Mời tham gia MISA AMIS</div>
                <div class="invite-box-close"></div>
            </div>
            <div class="invite-box-content">
                <div class="invite-box-des">Mời đồng nghiệp tham gia làm việc chung trên <strong>MISA AMIS</strong> để dễ dàng trao đổi công việc, gửi lịch họp và thông tin báo cáo.</div>
                <div class="invite-box-open-button">Gửi lời mời</div>
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

    /* Invite popup  */
    .invite-box-popup{

        width: 360px;
        position: fixed;
        top: 0;
        right: 0;
        left: auto;
        z-index: 5000000;
        float: left;
        cursor: default;
        color: #1f1f1f;
        border-radius: 16px;
        font-size: 14px;
        background: center / cover no-repeat url('../assets/icons/background.svg');
        box-shadow: 0 8px 12px rgba(0, 0, 0, .32);
        animation-name: fadeIn;

        padding: 20px;
    }
    .invite-box-header{
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding-bottom: 12px;
    }

    .invite-box-header .invite-box-title{
        font-weight: 700;
        font-size: 17px;
        line-height: 24px;
        letter-spacing: 0px;
        vertical-align: middle;
        margin-bottom: 0;
        padding-left: 32px;
        background: left / 24px no-repeat url('../assets/icons/invitebox-icon.svg')
    }

    .invite-box-close{
        position: absolute;
        top: 18px;
        right: 12px;
        width: 28px;
        height: 28px;
        cursor: pointer;
        background-color: transparent;
        border: none;
        font-size: 18px;
        color: #666;
        display: flex;
        align-items: center;
        justify-content: center;
        background-image: url('../assets/icons/close-icon.svg');
        background-position: center;
        background-repeat: no-repeat;
        background-size: 24px;
    }

    .invite-box-content{
        position: relative;
    }

    .invite-box-content .invite-box-des{
        position: relative;
        margin-bottom: 12px;
        line-height: 19px;
    }

    .invite-box-content .invite-box-open-button{
        display: flex;
        height: 32px;
        min-width: 80px;
        padding: 0 12px;
        justify-content: center;
        align-items: center;
        align-self: stretch;
        border-radius: 8px;
        background: #2563eb;
        color: #fff;
        font-weight: 500;
        cursor: pointer;
        -webkit-user-select: none;
        user-select: none;
    }
    

</style>