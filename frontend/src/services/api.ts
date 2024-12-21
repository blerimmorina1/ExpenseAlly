import axios from 'axios';
import { useAuthStore } from '@/stores/auth';

const api = axios.create({
  baseURL: 'http://localhost:7262/api', // Replace with your API base URL
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add a request interceptor
api.interceptors.request.use((config) => {
  const authStore = useAuthStore();
  if (authStore.token) {
    config.headers.Authorization = `Bearer ${authStore.token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

// Add a response interceptor
api.interceptors.response.use((response) => {
  return response;
}, (error) => {
  // Handle token expiration or other errors globally
  if (error.response?.status === 401) {
    const authStore = useAuthStore();
    authStore.clearToken();
  }
  return Promise.reject(error);
});

export default api;
