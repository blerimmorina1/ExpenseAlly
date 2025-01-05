<script setup lang="ts">
import FloatingConfigurator from '@/components/FloatingConfigurator.vue';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';
import { useToast } from 'primevue/usetoast';
import api from '@/services/ApiService';

const email = ref('');
const password = ref('');
const checked = ref(false);
const loading = ref(false);

const toast = useToast();
const authStore = useAuthStore();
const router = useRouter();

const handleLogin = async () => {
  loading.value = true;

  try {
      const response = await api.post('/account/login', {
        email: email.value,
        password: password.value,
      });

      authStore.setToken(response.data.accessToken);
      authStore.setRefreshToken(response.data.refreshToken);

      if (response.data.success) {
          localStorage.setItem('user', JSON.stringify(response.data.data));
          router.push('/');
      } else {
          toast.add({ severity: 'error', summary: 'Error', detail: response?.data?.errors[0].message || 'Login failed', life: 3000 });
      }
  } catch (error: any) {
      toast.add({ severity: 'error', summary: 'Error', detail: error.response?.data?.message || 'Login failed', life: 3000, });
  } finally {
      loading.value = false;
  }
};
</script>

<template>
  <FloatingConfigurator />
  <div class="bg-surface-50 dark:bg-surface-950 flex items-center justify-center min-h-screen min-w-[100vw] overflow-hidden">
    <div class="flex flex-col items-center justify-center">
      <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
        <div class="w-full bg-surface-0 dark:bg-surface-900 py-20 px-8 sm:px-20" style="border-radius: 53px">
          <div class="text-center mb-8">
            <div class="text-surface-900 dark:text-surface-0 text-3xl font-medium mb-4">ExpenseAlly</div>
            <span class="text-muted-color font-medium">Sign in to continue</span>
          </div>

          <form @submit.prevent="handleLogin">
            <label for="email1" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">Email</label>
            <InputText id="email1" type="text" placeholder="Email address" class="w-full md:w-[30rem] mb-4" v-model="email" />

            <label for="password1" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">Password</label>
            <Password id="password1" v-model="password" placeholder="Password" :toggleMask="true" class="mb-4" fluid :feedback="false"></Password>

            <div class="flex items-center justify-between mt-2 mb-8 gap-8">
              <div class="flex items-center">
                <Checkbox v-model="checked" id="rememberme1" binary class="mr-2"></Checkbox>
                <label for="rememberme1">Remember me</label>
              </div>
              <span class="font-medium no-underline ml-2 text-right cursor-pointer text-primary">Forgot password?</span>
            </div>
            <Button type="submit" icon="pi pi-user" label="Sign In" class="w-full" :loading="loading" />
          </form>

          <div class="text-center mt-6">
            <p class="text-muted-color font-medium">
              Don't have an account?
              <router-link to="/auth/signup" class="text-primary font-medium hover:underline">Sign Up</router-link>
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.pi-eye {
  transform: scale(1.6);
  margin-right: 1rem;
}

.pi-eye-slash {
  transform: scale(1.6);
  margin-right: 1rem;
}
</style>
