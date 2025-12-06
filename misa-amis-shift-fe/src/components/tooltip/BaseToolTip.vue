<script setup>
    const props = defineProps({
        content:{
            type: [String,Number],
            default: ""
        },
        placement: {
            type: String,
            default: "bottom"
        },
        effect: {
            type: String,
            default: "dark"
        },
        showAfter: {
            type: Number,
            default: 200 
        },
        showArror: {
            type: Boolean,
            default: true 
        },
    })
</script>

<template>
    <el-tooltip
        class="base-tootip"
        :effect="props.effect"
        :content="props.content"
        :placement="props.placement"
        :show-after="props.showAfter" 
        :show-arrow="props.showArror"
    >
        <template #content> 
            <slot name="content"></slot>    
        </template>
        
        <slot name="title"></slot>
    </el-tooltip>
</template>

<style>
.el-popper.is-dark {
    background: #111827 !important;
    color: #fff;
    font-size: 13px;
    font-weight: 500;
}

.el-popper.is-dark>.el-popper__arrow:before{
    background: #111827 !important;
}
</style>