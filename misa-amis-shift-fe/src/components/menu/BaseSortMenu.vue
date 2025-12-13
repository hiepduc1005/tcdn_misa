<script setup>
import { computed, ref } from 'vue';
import BasePopover from '../overlay/BasePopover.vue';

const props = defineProps({
    parentEl: {
        type: HTMLElement,
        required: true
    },
    showPopup: {
        type: Boolean,
        default: false
    },
    field: {
        type: String,
        default: ''
    },
    actionKey: {
        type: String,
        default: ''
    }

})

const emit = defineEmits(['close','action-key','pin','unpin']);

const sorts = [
    {
        label: "Không sắp xếp",
        key: "none",
        icon: "inactive-icon",
    },
    {
        label: "Tăng dần",
        key: "ASC",
        icon: "arrow-up-icon",
    },
    {
        label: "Giảm dần",
        key: "DESC",
        icon: "arrow-down-icon",
    }
];

const currentSortAction = computed({
    get: () => props.actionKey,
    set: (val) => emit('action-key', val,props.field)
})

const handleCliclSortAction = (key) => {
    currentSortAction.value = key
    
}

</script>

<template>
    <BasePopover
        :show="props.showPopup"
        :parentEl="props.parentEl"
        @close="emit('close')"
    >
        <template #content>
            <div class="sort-menu">
                <div 
                    class="sort-item flex flex-row align-center"
                    v-for="sort in sorts"
                    :key="sort.key"
                    :class="(currentSortAction === sort.key && currentSortAction !== 'none' ) ? 'sort-active' : '' "
                    @click="() => handleCliclSortAction(sort.key)"
                >
                    <div :class="sort.icon" style="margin: 0;" class="icon16"></div>
                    <div class="sort-icon-content">{{ sort.label  }}</div>
                </div>
                <div class="menu-border"></div>
                <div class="sort-item flex flex-row align-center" @click="() => emit('pin')">
                    <div style="margin: 0;" class="icon16 pin-icon"></div>
                    <div class="sort-icon-content">Ghim cột</div>
                </div>
                <div class="sort-item flex flex-row align-center" @click="() => emit('unpin')">
                    <div style="margin: 0;" class="icon16 unpin-icon"></div>
                    <div class="sort-icon-content">Bỏ ghim cột</div>
                </div>
            </div>
        </template>
    </BasePopover>
</template>


<style scoped>
    .sort-menu{
        animation: slide-down .2s ease;
        z-index: 9999;
        background-color: #fff;
        margin: 0;
    }

    .sort-item{
        white-space: nowrap;
        outline: none;
        padding: 8px 12px;
        color: inherit;
        text-decoration: none;
        height: 32px;
        cursor: pointer;
        transition: all .7s ease;
        -moz-column-gap: 8px;
        column-gap: 8px;
    }

    .sort-item:hover{
        outline: 0;
        background-color: #f3f4f6;
        border-radius: 2px;
        transition: all .2s ease;
    }

    .sort-active{
        color: #009b71;
        background-color: #dcfce7;
    }

    .sort-active .icon16{
        background-color: #009b71;
    }

    .menu-border {
        position: relative;
        display: flex;
        margin: 8px 12px;
        flex-direction: column;
        align-items: flex-start;
        align-self: stretch;
        height: 1px;
        background: #d1d5db;
    }

</style>