import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { useAuthStore } from '@/stores/auth';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

let isRefreshing = false;
let failedQueue: { resolve: (value: unknown) => void; reject: (error: unknown) => void }[] = [];

// Process failed requests queue
const processQueue = (token: string | null, error: AxiosError | null) => {
  failedQueue.forEach((promise) => {
    if (token) {
      promise.resolve(token);
    } else {
      promise.reject(error);
    }
  });
  failedQueue = [];
};

// Request interceptor
api.interceptors.request.use(
  (config: AxiosRequestConfig) => {
    const authStore = useAuthStore();
    if (authStore.token) {
      config.headers = {
        ...config.headers,
        Authorization: `Bearer ${authStore.token}`,
      };
    }
    return config;
  },
  (error: AxiosError) => Promise.reject(error)
);

// Response interceptor
api.interceptors.response.use(
  (response: AxiosResponse) => response,
  async (error: AxiosError) => {
    const authStore = useAuthStore();
    const originalRequest = error.config as AxiosRequestConfig & { _retry?: boolean };

    // Handle 401 errors
    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        }).then((token) => {
          originalRequest.headers = {
            ...originalRequest.headers,
            Authorization: `Bearer ${token}`,
          };
          return axios(originalRequest);
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const newToken = await authStore.refreshAccessToken();

        if (newToken) {
          api.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
          processQueue(newToken, null);
          return api(originalRequest);
        }
      } catch (refreshError) {
        processQueue(null, refreshError as AxiosError);
        authStore.clearToken(); // Logout if refresh fails
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export default api;
