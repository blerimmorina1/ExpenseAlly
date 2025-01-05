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
      {
        path: '/budgets',
        name: 'Budgets',
        component: () => import('@/views/pages/BudgetsView.vue'),
      },
    ]
  },
  {
    path: '/pages/auth/empty',
    name: 'empty',
    component: () => import('@/views/pages/auth/Empty.vue')
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'notfound',
    component: () => import('@/views/pages/auth/NotFound.vue')
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
