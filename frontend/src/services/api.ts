import axios from 'axios';

// The baseURL should eventually be moved to an .env file (.env.development)
export const api = axios.create({
  baseURL: 'http://localhost:5245/api/v1', 
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request Interceptor: Injects the JWT token into every outgoing request
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('jwt_token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response Interceptor: Handles global API errors, specifically 401 Unauthorized
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response && error.response.status === 401) {
      // Clears invalid token and forces redirection to the authentication route
      localStorage.removeItem('jwt_token');
      window.location.href = '/login'; 
    }
    return Promise.reject(error);
  }
);