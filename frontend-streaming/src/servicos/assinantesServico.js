import api from "./api";

const listar = (params) => api.get("/assinantes", { params });
const buscarPorId = (id) => api.get(`/assinantes/${id}`);
const criar = (dados) => api.post("/assinantes", dados);
const atualizar = (id, dados) => api.put(`/assinantes/${id}`, dados);
const excluir = (id) => api.delete(`/assinantes/${id}`);

export default { listar, buscarPorId, criar, atualizar, excluir };
