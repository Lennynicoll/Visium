import api from './axiosConfig';

const endpoint = '/Documentos';

export const getAll = () => api.get(endpoint);
export const getById = (id) => api.get(`${endpoint}/${id}`);
export const create = (data) => api.post(endpoint, data);
export const update = (id, data) => api.put(`${endpoint}/${id}`, data);
export const remove = (id) => api.delete(`${endpoint}/${id}`);
export const getByVisitante = (visitanteId) => api.get(`${endpoint}/visitante/${visitanteId}`);
