<script setup lang="ts">
import FloatingConfigurator from '@/components/FloatingConfigurator.vue';
import { ref } from 'vue';
import api from '@/services/ApiService';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';
import { useToast } from 'primevue/usetoast';

const first_name = ref('');
const last_name = ref('');
const email = ref('');
const password = ref('');
const confirm_password = ref('');
const loading = ref(false);
const router = useRouter();
const toast = useToast();
const authStore = useAuthStore();

const handleSignup = async () => {
    if (password.value !== confirm_password.value) {
        toast.add({ severity: 'warn', summary: 'Warning', detail: 'Passwords do not match!', life: 3000 });
        return;
    }

  loading.value = true;

  try {
      const response = await api.post('/account/register', {
          firstName: first_name.value,
          lastName: last_name.value,
          email: email.value,
          password: password.value,
          confirmPassword: confirm_password.value
      });

      if (response.data.success) {
          toast.add({ severity: 'success', summary: 'Success', detail: 'Account created successfully!', life: 3000 });
          router.push('/auth/login');
      } else {
          toast.add({ severity: 'error', summary: 'Error', detail: response?.data?.errors[0].message || 'Signup failed', life: 3000 });
      }
  } catch (error: any) {
       toast.add({ severity: 'error', summary: 'Error', detail: error.response?.data?.message || 'Signup failed', life: 3000, });
  } finally {
       loading.value = false;
  }
};
</script>

<template>
  <FloatingConfigurator />
  <Toast />
  <div class="bg-surface-50 dark:bg-surface-950 flex items-center justify-center min-h-screen min-w-[100vw] overflow-hidden">
    <div class="flex flex-col items-center justify-center">
      <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
        <div class="w-full bg-surface-0 dark:bg-surface-900 py-20 px-8 sm:px-20" style="border-radius: 53px">
          <div class="text-center mb-8">
            <div class="text-surface-900 dark:text-surface-0 text-3xl font-medium mb-4">ExpenseAlly</div>
            <span class="text-muted-color font-medium">Sign up to continue</span>
          </div>

          <div>
            <label for="firstname" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">First name</label>
            <InputText id="firstname" type="text" placeholder="First name" class="w-full md:w-[30rem] mb-4" v-model="first_name" />

            <label for="lastname" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">Last name</label>
            <InputText id="lastname" type="text" placeholder="Last name" class="w-full md:w-[30rem] mb-4" v-model="last_name" />

            <label for="email1" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">Email</label>
            <InputText id="email1" type="text" placeholder="Email address" class="w-full md:w-[30rem] mb-4" v-model="email" />

            <label for="password1" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">Password</label>
            <Password id="password1" v-model="password" placeholder="Password" :toggleMask="true" class="mb-4" fluid :feedback="false"></Password>

            <label for="password2" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">Confirm password</label>
            <Password id="password2" v-model="confirm_password" placeholder="Confirm password" :toggleMask="true" class="mb-4" fluid :feedback="false"></Password>

            <Button label="Sign Up" class="w-full mt-8" :loading="loading" @click="handleSignup" />
          </div>

          <div class="text-center mt-6">
            <p class="text-muted-color font-medium">
              Already have an account?
              <router-link to="/auth/login" class="text-primary font-medium hover:underline">Log In</router-link>
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
