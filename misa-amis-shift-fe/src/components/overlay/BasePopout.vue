<script setup>
    import { useRouter } from 'vue-router';

    const props = defineProps({
        item: {
            type: Object,
            default: {}
        },
        hoveringIndex: [String, Number],
        onMouseEnterPopout: {
            type: Function,
            default: () => {}
        },
        popoutStyle: Object,
        onMouseLeave: {
            type: Function,
            default: () => {}
        }
    })

    const router = useRouter();

    const handleClick = (item) => {
        if(item.path){
            router.push(item.path);
        }
    }
</script>
<template>
    <Teleport to="body">
        <div 
            v-if="props.item.type === 'popout' && props.item.children && props.hoveringIndex === props.item.index" 
            class="sidebar__popout"
            :style="props.popoutStyle"
            @mouseenter="props.onMouseEnterPopout"
            @mouseleave="props.onMouseLeave"
        >
            
            <div class="popout__body flex flex-row content-center">
                <div v-for="child in props.item.children" :key="child.key" class="popout__item flex flex-column">
                    <div class="label flex flex-row align-center" v-if="child?.label">
                        <span>{{ child?.label }}</span>
                    </div>
                    <div class="flex flex-column pointer">
                        <div 
                            style="gap: 4px; height: 32px;" 
                            v-for="popoutBodyItem in child.items" 
                            :key="popoutBodyItem.index" 
                            class="body-item flex flex-row align-center"
                            @click="handleClick(popoutBodyItem)"
                        >
                            <div class="icon icon-turndown"></div>
                            <span>{{ popoutBodyItem.title }}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </Teleport>
</template>

<style scoped>
    .sidebar__item-wrapper {
        position: relative; 

    }

    .sidebar__popout {
        position: fixed; /* Bắt buộc là Fixed khi dùng Teleport */
        z-index: 99999;  /* Cao hẳn lên để đè lên mọi thứ */
        background: #111827;
        border-radius: 4px;
       box-shadow: 0 0 8px #0000001a, 0 8px 16px #0000001a;
        border: 1px solid #374151;
        animation: popoutFadeIn 0.2s ease-out forwards;        
        pointer-events: auto; 
        width: auto;
    }

    .sidebar__item-wrapper:hover .sidebar__popout {
        opacity: 1;
        visibility: visible;
        transform: translateX(0);
    }

    .popout__header {
        background-color: #111827; 
        padding: 12px 16px;
        color: #fff;
        font-weight: 600;
        font-size: 14px;
        display: flex;
        align-items: center;
        gap: 10px;
    }
    .popout__header .icon {
        background-color: #fff; 
    }

    .popout__body {
        padding: 6px 6px;
        column-gap: 8px;
        gap: 4px;
    }

    .popout__body .label{
        height: 32px;
        color: #4b5563;
        font-weight: 500;
        font-size: 13px;
        width: 100%;
        line-height: 14px;
        padding: 0 32px;
        margin-bottom: 4px;
    }

    .popout__item {
        color: #9ca3af;
        font-size: 13px;
        transition: background-color 0.2s;
        cursor: default;
    }

    .body-item{
        border-radius: 4px;
    }

    .body-item:hover{
        background-color: #252c3b;
        color: #fff;
        cursor: pointer;
    }

    .body-item:hover .icon{
        background-color: #fff;
    }

    
</style>