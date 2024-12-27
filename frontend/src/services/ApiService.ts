import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { useAuthStore } from '@/stores/auth';

const apiService = axios.create({
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
apiService.interceptors.request.use(
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
apiService.interceptors.response.use(
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
          apiService.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
          processQueue(newToken, null);
          return apiService(originalRequest);
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

export default apiService;
