import axios from "axios";

const baseURL = "http://localhost:5198";

const api = axios.create({
    baseURL: baseURL,
    headers: {
        'Content-Type': 'application/json'
    }
})


api.interceptors.response.use((response) => response, (error) => {
    console.error('API error', error);
    return Promise.reject(error);
})

export default api;