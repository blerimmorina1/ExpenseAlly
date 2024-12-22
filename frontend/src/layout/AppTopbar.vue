<script setup>
import { ref } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import AppConfigurator from './AppConfigurator.vue';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';

const { toggleMenu, toggleDarkMode, isDarkTheme } = useLayout();
const authStore = useAuthStore();
const router = useRouter();
const isMenuOpen = ref(false);

const toggleProfileMenu = () => {
  isMenuOpen.value = !isMenuOpen.value;
};

const logout = () => {
  authStore.clearToken();
  router.push('/auth/login');
  isMenuOpen.value = false;
};
</script>

<template>
    <div class="layout-topbar">
        <div class="layout-topbar-logo-container">
            <button class="layout-menu-button layout-topbar-action" @click="toggleMenu">
                <i class="pi pi-bars"></i>
            </button>
            <router-link to="/" class="layout-topbar-logo">
                <span>ExpenseAlly</span>
            </router-link>
        </div>

        <div class="layout-topbar-actions">
            <div class="layout-config-menu">
                <button type="button" class="layout-topbar-action" @click="toggleDarkMode">
                    <i :class="['pi', { 'pi-moon': isDarkTheme, 'pi-sun': !isDarkTheme }]"></i>
                </button>
                <div class="relative">
                    <button
                        v-styleclass="{ selector: '@next', enterFromClass: 'hidden', enterActiveClass: 'animate-scalein', leaveToClass: 'hidden', leaveActiveClass: 'animate-fadeout', hideOnOutsideClick: true }"
                        type="button"
                        class="layout-topbar-action layout-topbar-action-highlight"
                    >
                        <i class="pi pi-palette"></i>
                    </button>
                    <AppConfigurator />
                </div>
            </div>

            <button
                class="layout-topbar-menu-button layout-topbar-action"
                v-styleclass="{ selector: '@next', enterFromClass: 'hidden', enterActiveClass: 'animate-scalein', leaveToClass: 'hidden', leaveActiveClass: 'animate-fadeout', hideOnOutsideClick: true }"
            >
                <i class="pi pi-ellipsis-v"></i>
            </button>

            <div class="layout-topbar-menu hidden lg:block">
                <div class="layout-topbar-menu-content">
                    <button type="button" class="layout-topbar-action">
                        <i class="pi pi-calendar"></i>
                        <span>Calendar</span>
                    </button>
                    <button type="button" class="layout-topbar-action">
                        <i class="pi pi-inbox"></i>
                        <span>Messages</span>
                    </button>
                    <div class="relative inline-block text-left menu-container">
                        <button
                          type="button"
                          class="layout-topbar-action flex items-center gap-2"
                          @click="toggleProfileMenu"
                        >
                          <i class="pi pi-user"></i>
                          <span>Profile</span>
                        </button>

                        <div
                          v-show="isMenuOpen"
                          class="absolute right-0 z-10 mt-2 w-48 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded-lg shadow-lg"
                        >
                            <ul class="py-1">
                                <li
                                  class="px-4 py-2 text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700 cursor-pointer flex items-center gap-2 transition-colors duration-150"
                                >
                                  <i class="pi pi-user"></i> Profile
                                </li>
                                <li
                                  @click="logout"
                                  class="px-4 py-2 text-red-500 dark:text-red-400 hover:bg-gray-100 dark:hover:bg-gray-700 cursor-pointer flex items-center gap-2 transition-colors duration-150"
                                >
                                  <i class="pi pi-sign-out"></i> Logout
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
