<script setup>
import { ref } from 'vue';

import AppMenuItem from './AppMenuItem.vue';

const model = ref([
    {
        items: [
            {
                label: 'Dashboard',
                icon: 'pi pi-fw pi-home',
                to: '/'
            },
          {
                label: 'Categories',
                icon: 'pi pi-fw pi-receipt',
                to: '/categories'
            },
            {
                label: 'Transactions',
                icon: 'pi pi-fw pi-dollar',
                to: '/transactions'
            },
            {
                label: 'Budgets',
                icon: 'pi pi-fw pi-wallet',
                to: '/budgets'
            },
            {
                label: 'Saving Goals',
                icon: 'pi pi-fw pi-bullseye',
                to: '/saving-goals'
            },
        ]
    }
]);
</script>

<template>
    <ul class="layout-menu">
        <template v-for="(item, i) in model" :key="item">
            <app-menu-item v-if="!item.separator" :item="item" :index="i"></app-menu-item>
            <li v-if="item.separator" class="menu-separator"></li>
        </template>
    </ul>
</template>

<style lang="scss" scoped></style>
