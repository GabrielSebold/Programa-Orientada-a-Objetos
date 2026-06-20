import api from "./api";

const listar = () => api.get("/planos");
const buscarPorId = (id) => api.get(`/planos/${id}`);
const criar = (dados) => api.post("/planos", dados);
const atualizar = (id, dados) => api.put(`/planos/${id}`, dados);
const excluir = (id) => api.delete(`/planos/${id}`);

export default { listar, buscarPorId, criar, atualizar, excluir };
