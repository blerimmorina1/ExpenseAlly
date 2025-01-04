import AppLayout from '@/layout/AppLayout.vue';
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: AppLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: '/',
        name: 'dashboard',
        component: () => import('@/views/Dashboard.vue'),
      },
      {
        path: '/uikit/formlayout',
        name: 'formlayout',
        component: () => import('@/views/uikit/FormLayout.vue')
      },
      {
        path: '/uikit/input',
        name: 'input',
        component: () => import('@/views/uikit/InputDoc.vue')
      },
      {
        path: '/uikit/button',
        name: 'button',
        component: () => import('@/views/uikit/ButtonDoc.vue')
      },
      {
        path: '/uikit/table',
        name: 'table',
        component: () => import('@/views/uikit/TableDoc.vue')
      },
      {
        path: '/uikit/list',
        name: 'list',
        component: () => import('@/views/uikit/ListDoc.vue')
      },
      {
        path: '/uikit/tree',
        name: 'tree',
        component: () => import('@/views/uikit/TreeDoc.vue')
      },
      {
        path: '/uikit/panel',
        name: 'panel',
        component: () => import('@/views/uikit/PanelsDoc.vue')
      },
      {
        path: '/uikit/overlay',
        name: 'overlay',
        component: () => import('@/views/uikit/OverlayDoc.vue')
      },
      {
        path: '/uikit/media',
        name: 'media',
        component: () => import('@/views/uikit/MediaDoc.vue')
      },
      {
        path: '/uikit/message',
        name: 'message',
        component: () => import('@/views/uikit/MessagesDoc.vue')
      },
      {
        path: '/uikit/file',
        name: 'file',
        component: () => import('@/views/uikit/FileDoc.vue')
      },
      {
        path: '/uikit/menu',
        name: 'menu',
        component: () => import('@/views/uikit/MenuDoc.vue')
      },
      {
        path: '/uikit/charts',
        name: 'charts',
        component: () => import('@/views/uikit/ChartDoc.vue')
      },
      {
        path: '/uikit/misc',
        name: 'misc',
        component: () => import('@/views/uikit/MiscDoc.vue')
      },
      {
        path: '/uikit/timeline',
        name: 'timeline',
        component: () => import('@/views/uikit/TimelineDoc.vue')
      },
      {
        path: '/pages/empty',
        name: 'empty',
        component: () => import('@/views/pages/Empty.vue')
      },
      {
        path: '/pages/crud',
        name: 'crud',
        component: () => import('@/views/pages/Crud.vue')
      },
      {
        path: '/categories',
        name: 'Categories',
        component: () => import('@/views/pages/CategoriesView.vue'),
      },
      {
        path: '/transactions',
        name: 'Transactions',
        component: () => import('@/views/pages/TransactionsView.vue'),
      },
      {
        path: '/saving-goals',
        name: 'Saving Goals',
        component: () => import('@/views/pages/SavingGoalsView.vue'),
      },
    ]
  },
  {
    path: '/landing',
    name: 'landing',
    component: () => import('@/views/pages/Landing.vue')
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'notfound',
    component: () => import('@/views/pages/NotFound.vue')
  },
  {
    path: '/auth/login',
    name: 'login',
    component: () => import('@/views/pages/auth/Login.vue')
  },
  {
    path: '/auth/signup',
    name: 'signup',
    component: () => import('@/views/pages/auth/SignUp.vue')
  },
  {
    path: '/auth/error',
    name: 'error',
    component: () => import('@/views/pages/auth/Error.vue')
  },
  {
    path: '/auth/access',
    name: 'access',
    component: () => import('@/views/pages/auth/Access.vue')
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

// Navigation Guard for Authentication
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const isAuthenticated = authStore.isAuthenticated();

  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/auth/login');
  } else {
    next();
  }
});

export default router;
