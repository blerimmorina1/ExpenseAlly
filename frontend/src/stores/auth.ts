import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('auth_token'));

  const isAuthenticated = () => !!token.value;

  const setToken = (newToken: string) => {
    token.value = newToken;
    localStorage.setItem('auth_token', newToken);
  };

  const clearToken = () => {
    token.value = null;
    localStorage.removeItem('auth_token');
  };

  const loadTokenFromStorage = () => {
    const storedToken = localStorage.getItem('auth_token');
    if (storedToken) {
      token.value = storedToken;
    }
  };

  return { token, setToken, clearToken, isAuthenticated, loadTokenFromStorage };
});
