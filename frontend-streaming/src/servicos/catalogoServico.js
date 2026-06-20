import api from "./api";

const listar = (params) => api.get("/catalogo", { params });
const buscarPorId = (id) => api.get(`/catalogo/${id}`);
const criar = (dados) => api.post("/catalogo", dados);
const atualizar = (id, dados) => api.put(`/catalogo/${id}`, dados);
const excluir = (id) => api.delete(`/catalogo/${id}`);
const listarEmAlta = () => api.get("/catalogo/em-alta");

export default { listar, buscarPorId, criar, atualizar, excluir, listarEmAlta };
