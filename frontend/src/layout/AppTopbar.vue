<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import AppConfigurator from './AppConfigurator.vue';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';
import signalRService from '@/stores/signalRInstance';

const { toggleMenu, toggleDarkMode, isDarkTheme } = useLayout();
const authStore = useAuthStore();
const router = useRouter();
const isMenuOpen = ref(false);
const newNotificationCount = ref(0);  // Track new notification count
const showNotifications = ref(false); // Track the visibility of the notifications
const notifications = ref<Array<{ id: string, title: string; message: string; createdOn: string }>>([]); // Store notifications

// Function to toggle the visibility of the notification component
const toggleNotifications = () => {
    showNotifications.value = !showNotifications.value;
    if (showNotifications.value) {
        newNotificationCount.value = 0; // Reset the notification count when viewing the list
    }
};

const handleNotification = (notification: { id: string, title: string; message: string; createdOn: string }) => {
    debugger;
    notifications.value.push({
        id: notification.id, title: notification.title,
        message: notification.message, createdOn: notification.createdOn
    });
    newNotificationCount.value += 1; // Increment the count of new notifications
};

const toggleProfileMenu = () => {
    isMenuOpen.value = !isMenuOpen.value;
};

const logout = () => {
    authStore.clearToken();
    isMenuOpen.value = false;
};

const sortedNotifications = computed(() => {
    return notifications.value.slice().sort((a, b) => new Date(b.createdOn).getTime() - new Date(a.createdOn).getTime());
});

// Start SignalR connection and register notification handler
onMounted(async () => {
    await signalRService.startConnection();
    signalRService.registerNotificationHandler('ReceiveNotification', handleNotification);
});

onUnmounted(() => {
    signalRService.unregisterNotificationHandler('ReceiveNotification');
    signalRService.stopConnection();
});


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
                        type="button" class="layout-topbar-action layout-topbar-action-highlight">
                        <i class="pi pi-palette"></i>
                    </button>
                    <AppConfigurator />
                </div>
            </div>

            <button class="layout-topbar-menu-button layout-topbar-action"
                v-styleclass="{ selector: '@next', enterFromClass: 'hidden', enterActiveClass: 'animate-scalein', leaveToClass: 'hidden', leaveActiveClass: 'animate-fadeout', hideOnOutsideClick: true }">
                <i class="pi pi-ellipsis-v"></i>
            </button>

            <div class="layout-topbar-menu hidden lg:block">
                <div class="layout-topbar-menu-content">
                    <div class="relative inline-block text-left menu-container">
                        <button type="button" class="layout-topbar-action" @click="toggleNotifications">
                            <i class="pi pi-bell"></i>
                            <span>Notifications</span>
                            <span v-if="newNotificationCount > 0" class="notification-badge">{{ newNotificationCount
                                }}</span>
                        </button>
                        <!-- Notification List Component -->
                        <div v-show="showNotifications"
                            class="absolute right-0 z-10 mt-2 w-64 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded-lg shadow-lg">
                            <ul v-if="notifications.length" class="py-2">
                                <li v-for="notification in sortedNotifications" :key="notification.id"
                                    class="px-4 py-3 border-b last:border-none border-gray-100 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700 cursor-pointer">
                                    <div class="flex justify-between items-center mb-1">
                                        <span class="font-semibold text-gray-900 dark:text-gray-100">{{
                                            notification.title }}</span>
                                        <span class="text-xs text-gray-500 dark:text-gray-400">
                                            {{ new Date(notification.createdOn).toLocaleString() }}
                                        </span>
                                    </div>
                                    <p class="text-sm text-gray-700 dark:text-gray-300">
                                        {{ notification.message }}
                                    </p>
                                </li>
                            </ul>
                            <p v-else class="px-4 py-2 text-gray-500 dark:text-gray-300 text-center">
                                No notifications yet!
                            </p>
                        </div>
                    </div>

                    <div class="relative inline-block text-left menu-container">
                        <button type="button" class="layout-topbar-action flex items-center gap-2"
                            @click="toggleProfileMenu">
                            <i class="pi pi-user"></i>
                            <span>Profile</span>
                        </button>

                        <div v-show="isMenuOpen"
                            class="absolute right-0 z-10 mt-2 w-48 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded-lg shadow-lg">
                            <ul class="py-1">
                                <li
                                    class="px-4 py-2 text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700 cursor-pointer flex items-center gap-2 transition-colors duration-150">
                                    <i class="pi pi-user"></i> Profile
                                </li>
                                <li @click="logout"
                                    class="px-4 py-2 text-red-500 dark:text-red-400 hover:bg-gray-100 dark:hover:bg-gray-700 cursor-pointer flex items-center gap-2 transition-colors duration-150">
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

<style scoped>
.notification-badge {
    background-color: red;
    color: white;
    font-size: 14px;
    border-radius: 50%;
    padding: 0 6px;
    position: absolute;
    top: -5px;
    right: -5px;
}
</style>
