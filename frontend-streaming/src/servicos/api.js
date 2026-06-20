import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5083/api",
});

export default api;
