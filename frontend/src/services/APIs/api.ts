import axios from "axios";

export enum HttpCodes {
    'validation' = 422,
    'unauthorized' = 401,
    'thirdParty' = 433,
    'tooManyAttempts' = 429,
    'forbidden' = 403,
    'not_found' = 404,
    'locked' = 423,
    'conflict'=409,
    'payment_required'=402,
    'RequestEntityTooLarge'=413,
    'NO_CONTENT'=204
}
export enum Method {
    'get'='get',
    'post'='post',
    'put'='put',
    'delete'='delete',
}

const api = axios.create({
  baseURL: "https://localhost:7082/api",
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});


api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      console.warn("Unauthorized: Token expired or invalid");
      localStorage.removeItem("token");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

export default api;