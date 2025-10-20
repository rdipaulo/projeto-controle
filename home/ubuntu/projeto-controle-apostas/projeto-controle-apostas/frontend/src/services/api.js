import axios from 'axios';

const API_BASE_URL = 'http://localhost:5050/api';

// Criar instância do axios com configurações padrão
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Adicionar token JWT ao header de autorização
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Serviço de Autenticação
export const authService = {
  register: (name, email, password) =>
    api.post('/Auth/register', { name, email, password }),
  login: (email, password) =>
    api.post('/Auth/login', { email, password }),
};

// Serviço de Ciclos
export const cicloService = {
  getAll: () => api.get('/Ciclos'),
  getById: (id) => api.get(`/Ciclos/${id}`),
  create: (name, startDate) =>
    api.post('/Ciclos', { name, startDate }),
  update: (id, name, startDate, endDate) =>
    api.put(`/Ciclos/${id}`, { name, startDate, endDate }),
  delete: (id) => api.delete(`/Ciclos/${id}`),
  encerrar: (id) => api.put(`/Ciclos/encerrar/${id}`),
};

// Serviço de Apostas (Bets)
export const betService = {
  getAll: () => api.get('/Bets'),
  getById: (id) => api.get(`/Bets/${id}`),
  create: (bet) => api.post('/Bets', bet),
  update: (id, bet) => api.put(`/Bets/${id}`, bet),
  delete: (id) => api.delete(`/Bets/${id}`),
};

// Serviço de Análises
export const analysisService = {
  getRoiGeral: () => api.get('/Analises/roi-geral'),
  getRoiPorCiclo: () => api.get('/Analises/roi-por-ciclo'),
  getYield: () => api.get('/Analises/yield'),
  getTaxaAcerto: () => api.get('/Analises/taxa-acerto'),
  getLucroLiquido: () => api.get('/Analises/lucro-liquido'),
  getLucroAcumulado: () => api.get('/Analises/lucro-acumulado'),
  getHistoricoBanca: () => api.get('/Analises/historico-banca'),
  getAnaliseMercados: () => api.get('/Analises/analise-mercados'),
  getAnaliseCampeonatos: () => api.get('/Analises/analise-campeonatos'),
  getAnalisePaises: () => api.get('/Analises/analise-paises'),
  getAnaliseTimesLucrativos: () => api.get('/Analises/analise-times-lucrativos'),
  getAlertasInteligentes: () => api.get('/Analises/alertas-inteligentes'),
  getFechamentoMensal: (mes, ano) =>
    api.get(`/Analises/fechamento-mensal?mes=${mes}&ano=${ano}`),
};

export default api;

