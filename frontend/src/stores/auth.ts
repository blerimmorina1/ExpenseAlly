import { defineStore } from 'pinia';
import { ref } from 'vue';
import router from '@/router';
import axios from 'axios';
import { useToast } from 'primevue/usetoast';

export const useAuthStore = defineStore('auth', () => {
  // States
  const token = ref<string | null>(localStorage.getItem('auth_token'));
  const refreshToken = ref<string | null>(localStorage.getItem('refresh_token'));
  const toast = useToast();

  // Getters
  const isAuthenticated = () => !!token.value;

  // Actions
  const setToken = (newToken: string) => {
    token.value = newToken;
    localStorage.setItem('auth_token', newToken);
  };

  const setRefreshToken = (newRefreshToken: string) => {
    refreshToken.value = newRefreshToken;
    localStorage.setItem('refresh_token', newRefreshToken);
  };

  const clearToken = () => {
    token.value = null;
    refreshToken.value = null;
    localStorage.removeItem('auth_token');
    localStorage.removeItem('refresh_token');
    router.push('/auth/login');
  };

  const loadTokenFromStorage = () => {
    const storedToken = localStorage.getItem('auth_token');
    const storedRefreshToken = localStorage.getItem('refresh_token');
    if (storedToken) token.value = storedToken;
    if (storedRefreshToken) refreshToken.value = storedRefreshToken;
  };

  // Refresh Token Handler
  const refreshAccessToken = async () => {
    try {
      const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;
      const response = await axios.post(`${apiBaseUrl}/account/refreshToken`, {
        token: token.value,
        refreshToken: refreshToken.value,
      });

      // Update tokens
      setToken(response.data.token);
      setRefreshToken(response.data.refreshToken);

      return response.data.token;
    } catch (error) {
      console.error('Failed to refresh token:', error);
      toast.add({
        severity: 'error',
        summary: 'Unauthorized',
        detail: 'Session expired. Please login again.',
        life: 3000
      });
      clearToken(); // Clear session on failure
      return null;
    }
  };

  return { token, refreshToken, isAuthenticated, setToken, setRefreshToken, clearToken, loadTokenFromStorage, refreshAccessToken };
});
