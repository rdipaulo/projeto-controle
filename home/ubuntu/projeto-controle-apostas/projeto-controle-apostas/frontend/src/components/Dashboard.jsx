import { useState, useEffect } from 'react';
import { cicloService } from '../services/api';
import CicloForm from './CicloForm';
import CicloList from './CicloList';
import BetForm from './BetForm';
import AnalysisPanel from './AnalysisPanel';
import MonthlyClosing from './MonthlyClosing';
import '../styles/Dashboard.css';

function Dashboard({ onLogout }) {
  const [ciclos, setCiclos] = useState([]);
  const [selectedCiclo, setSelectedCiclo] = useState(null);
  const [showCicloForm, setShowCicloForm] = useState(false);
  const [showBetForm, setShowBetForm] = useState(false);
  const [activeTab, setActiveTab] = useState('ciclos'); // ciclos, analises, fechamento
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadCiclos();
  }, []);

  const loadCiclos = async () => {
    try {
      setLoading(true);
      const response = await cicloService.getAll();
      setCiclos(response.data);
      setError('');
    } catch (err) {
      setError('Erro ao carregar ciclos. Tente novamente.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCicloCreated = () => {
    setShowCicloForm(false);
    loadCiclos();
  };

  const handleBetCreated = () => {
    setShowBetForm(false);
    loadCiclos();
  };

  const handleCicloSelected = (ciclo) => {
    setSelectedCiclo(ciclo);
  };

  return (
    <div className="dashboard">
      <header className="dashboard-header">
        <h1>🎯 Controle de Apostas Esportivas</h1>
        <button onClick={onLogout} className="btn-logout">Sair</button>
      </header>

      <div className="dashboard-content">
        <div className="sidebar">
          <div className="tab-buttons">
            <button
              className={`tab-btn ${activeTab === 'ciclos' ? 'active' : ''}`}
              onClick={() => setActiveTab('ciclos')}
            >
              📊 Ciclos
            </button>
            <button
              className={`tab-btn ${activeTab === 'analises' ? 'active' : ''}`}
              onClick={() => setActiveTab('analises')}
            >
              📈 Análises
            </button>
            <button
              className={`tab-btn ${activeTab === 'fechamento' ? 'active' : ''}`}
              onClick={() => setActiveTab('fechamento')}
            >
              📋 Fechamento
            </button>
          </div>

          {activeTab === 'ciclos' && (
            <>
              <button
                onClick={() => setShowCicloForm(!showCicloForm)}
                className="btn-primary"
              >
                {showCicloForm ? 'Cancelar' : '➕ Novo Ciclo'}
              </button>

              {showCicloForm && (
                <CicloForm onSuccess={handleCicloCreated} />
              )}

              <h2>Ciclos</h2>
              {loading ? (
                <p>Carregando...</p>
              ) : error ? (
                <p className="error-message">{error}</p>
              ) : (
                <CicloList
                  ciclos={ciclos}
                  selectedCiclo={selectedCiclo}
                  onSelect={handleCicloSelected}
                  onRefresh={loadCiclos}
                />
              )}
            </>
          )}
        </div>

        <div className="main-content">
          {activeTab === 'ciclos' && (
            <>
              {selectedCiclo ? (
                <>
                  <h2>{selectedCiclo.name}</h2>
                  <div className="ciclo-info">
                    <p><strong>Início:</strong> {new Date(selectedCiclo.startDate).toLocaleDateString('pt-BR')}</p>
                    {selectedCiclo.endDate && (
                      <p><strong>Término:</strong> {new Date(selectedCiclo.endDate).toLocaleDateString('pt-BR')}</p>
                    )}
                    <p><strong>Total Apostado:</strong> R$ {selectedCiclo.totalApostado.toFixed(2)}</p>
                    <p><strong>Total Ganhos:</strong> R$ {selectedCiclo.totalGanhos.toFixed(2)}</p>
                    <p><strong>Lucro/Prejuízo:</strong> R$ {selectedCiclo.lucroPrejuizo.toFixed(2)}</p>
                    <p><strong>ROI:</strong> {selectedCiclo.roi.toFixed(2)}%</p>
                    <p><strong>Status:</strong> {selectedCiclo.isClosed ? '✓ Encerrado' : '● Aberto'}</p>
                  </div>

                  <button
                    onClick={() => setShowBetForm(!showBetForm)}
                    className="btn-primary"
                    disabled={selectedCiclo.isClosed}
                  >
                    {showBetForm ? 'Cancelar' : '➕ Nova Aposta'}
                  </button>

                  {showBetForm && (
                    <BetForm
                      cicloId={selectedCiclo.id}
                      onSuccess={handleBetCreated}
                    />
                  )}

                  <div className="bets-list">
                    <h3>Apostas do Ciclo</h3>
                    {selectedCiclo.bets && selectedCiclo.bets.length > 0 ? (
                      <table>
                        <thead>
                          <tr>
                            <th>Data</th>
                            <th>Esporte</th>
                            <th>Campeonato</th>
                            <th>Times</th>
                            <th>Mercado</th>
                            <th>Odd</th>
                            <th>Valor</th>
                            <th>Resultado</th>
                            <th>Lucro/Prejuízo</th>
                          </tr>
                        </thead>
                        <tbody>
                          {selectedCiclo.bets.map((bet) => (
                            <tr key={bet.id}>
                              <td>{new Date(bet.dataAposta).toLocaleDateString('pt-BR')}</td>
                              <td>{bet.tipoEsporte === 0 ? '⚽ Futebol' : '🏀 Basquete'}</td>
                              <td>{bet.campeonato}</td>
                              <td>{bet.timeCasa} vs {bet.timeVisitante}</td>
                              <td>{bet.mercado}</td>
                              <td>{bet.odd.toFixed(2)}</td>
                              <td>R$ {bet.valorApostado.toFixed(2)}</td>
                              <td>
                                {bet.resultado === 0 ? '⏳ Pendente' : bet.resultado === 1 ? '✅ Ganhou' : '❌ Perdeu'}
                              </td>
                              <td className={bet.lucroPrejuizo >= 0 ? 'positive' : 'negative'}>
                                R$ {bet.lucroPrejuizo.toFixed(2)}
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    ) : (
                      <p>Nenhuma aposta neste ciclo.</p>
                    )}
                  </div>
                </>
              ) : (
                <div className="no-selection">
                  <p>👈 Selecione um ciclo na barra lateral para visualizar detalhes.</p>
                </div>
              )}
            </>
          )}

          {activeTab === 'analises' && (
            <AnalysisPanel />
          )}

          {activeTab === 'fechamento' && (
            <MonthlyClosing />
          )}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;

