import api from "./api";

const listar = (assinanteId) => api.get(`/assinantes/${assinanteId}/perfis`);
const criar = (assinanteId, dados) => api.post(`/assinantes/${assinanteId}/perfis`, dados);
const excluir = (assinanteId, perfilId) => api.delete(`/assinantes/${assinanteId}/perfis/${perfilId}`);

export default { listar, criar, excluir };
