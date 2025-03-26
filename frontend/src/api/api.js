import axios from "axios";

const API = axios.create({
    baseURL: "http://localhost:5070/api", // Backend URL
});

// Example API Calls:
export const fetchPosts = () => API.get("/posts");
export const loginUser = (credentials) => API.post("/auth/login", credentials);

export default API;
