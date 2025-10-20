import { useState } from 'react';
import { analysisService } from '../services/api';
import '../styles/MonthlyClosing.css';

function MonthlyClosing() {
  const [mes, setMes] = useState(new Date().getMonth() + 1);
  const [ano, setAno] = useState(new Date().getFullYear());
  const [fechamento, setFechamento] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleLoadFechamento = async () => {
    try {
      setLoading(true);
      setError('');
      const response = await analysisService.getFechamentoMensal(mes, ano);
      setFechamento(response.data);
    } catch (err) {
      setError('Erro ao carregar fechamento mensal. Tente novamente.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const meses = [
    'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
    'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
  ];

  const anos = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i);

  return (
    <div className="monthly-closing">
      <h2>Fechamento Mensal</h2>

      <div className="closing-filters">
        <div className="filter-group">
          <label htmlFor="mes">Mês:</label>
          <select
            id="mes"
            value={mes}
            onChange={(e) => setMes(parseInt(e.target.value))}
          >
            {meses.map((m, idx) => (
              <option key={idx} value={idx + 1}>
                {m}
              </option>
            ))}
          </select>
        </div>

        <div className="filter-group">
          <label htmlFor="ano">Ano:</label>
          <select
            id="ano"
            value={ano}
            onChange={(e) => setAno(parseInt(e.target.value))}
          >
            {anos.map((a) => (
              <option key={a} value={a}>
                {a}
              </option>
            ))}
          </select>
        </div>

        <button onClick={handleLoadFechamento} className="btn-load">
          Carregar Fechamento
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {loading && <div className="loading">Carregando...</div>}

      {fechamento && (
        <div className="closing-content">
          <div className="closing-header">
            <h3>{meses[fechamento.mes - 1]} de {fechamento.ano}</h3>
          </div>

          <div className="closing-summary">
            <div className="summary-card">
              <h4>Total de Apostas</h4>
              <p className="value">{fechamento.totalApostas}</p>
            </div>

            <div className="summary-card">
              <h4>Apostas Ganhas</h4>
              <p className="value positive">{fechamento.apostasGanhas}</p>
            </div>

            <div className="summary-card">
              <h4>Apostas Perdidas</h4>
              <p className="value negative">{fechamento.apostasPerdidas}</p>
            </div>

            <div className="summary-card">
              <h4>Apostas Pendentes</h4>
              <p className="value">{fechamento.apostasPendentes}</p>
            </div>

            <div className="summary-card">
              <h4>Taxa de Acerto</h4>
              <p className={`value ${fechamento.taxaAcerto >= 50 ? 'positive' : 'negative'}`}>
                {fechamento.taxaAcerto.toFixed(2)}%
              </p>
            </div>

            <div className="summary-card">
              <h4>ROI</h4>
              <p className={`value ${fechamento.roi >= 0 ? 'positive' : 'negative'}`}>
                {fechamento.roi.toFixed(2)}%
              </p>
            </div>

            <div className="summary-card">
              <h4>Yield</h4>
              <p className={`value ${fechamento.yield >= 0 ? 'positive' : 'negative'}`}>
                {fechamento.yield.toFixed(2)}%
              </p>
            </div>

            <div className="summary-card">
              <h4>Total Apostado</h4>
              <p className="value">R$ {fechamento.totalApostado.toFixed(2)}</p>
            </div>

            <div className="summary-card">
              <h4>Lucro/Prejuízo</h4>
              <p className={`value ${fechamento.lucro >= 0 ? 'positive' : 'negative'}`}>
                R$ {fechamento.lucro.toFixed(2)}
              </p>
            </div>

            <div className="summary-card">
              <h4>Banca Inicial</h4>
              <p className="value">R$ {fechamento.bancaInicial.toFixed(2)}</p>
            </div>

            <div className="summary-card">
              <h4>Banca Final</h4>
              <p className={`value ${fechamento.bancaFinal >= fechamento.bancaInicial ? 'positive' : 'negative'}`}>
                R$ {fechamento.bancaFinal.toFixed(2)}
              </p>
            </div>

            <div className="summary-card full-width">
              <h4>Variação da Banca</h4>
              <p className={`value ${fechamento.variacao >= 0 ? 'positive' : 'negative'}`}>
                {fechamento.variacao >= 0 ? '+' : ''}{fechamento.variacao.toFixed(2)}%
              </p>
            </div>
          </div>

          {/* Detalhes por Esporte */}
          {fechamento.detalhesPorEsporte && fechamento.detalhesPorEsporte.length > 0 && (
            <div className="closing-details">
              <h3>Detalhes por Esporte</h3>
              <div className="details-grid">
                {fechamento.detalhesPorEsporte.map((esporte, idx) => (
                  <div key={idx} className="detail-card">
                    <h4>{esporte.esporte === 0 ? '⚽ Futebol' : '🏀 Basquete'}</h4>
                    <div className="detail-item">
                      <span>Apostas:</span>
                      <strong>{esporte.totalApostas}</strong>
                    </div>
                    <div className="detail-item">
                      <span>Ganhas:</span>
                      <strong className="positive">{esporte.apostasGanhas}</strong>
                    </div>
                    <div className="detail-item">
                      <span>Taxa Acerto:</span>
                      <strong className={esporte.taxaAcerto >= 50 ? 'positive' : 'negative'}>
                        {esporte.taxaAcerto.toFixed(2)}%
                      </strong>
                    </div>
                    <div className="detail-item">
                      <span>Lucro:</span>
                      <strong className={esporte.lucro >= 0 ? 'positive' : 'negative'}>
                        R$ {esporte.lucro.toFixed(2)}
                      </strong>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* Detalhes por Mercado */}
          {fechamento.detalhesPorMercado && fechamento.detalhesPorMercado.length > 0 && (
            <div className="closing-details">
              <h3>Detalhes por Mercado</h3>
              <div className="details-table">
                <table>
                  <thead>
                    <tr>
                      <th>Mercado</th>
                      <th>Apostas</th>
                      <th>Ganhas</th>
                      <th>Taxa Acerto</th>
                      <th>ROI</th>
                      <th>Lucro</th>
                    </tr>
                  </thead>
                  <tbody>
                    {fechamento.detalhesPorMercado.map((mercado, idx) => (
                      <tr key={idx}>
                        <td>{mercado.mercado}</td>
                        <td>{mercado.totalApostas}</td>
                        <td>{mercado.apostasGanhas}</td>
                        <td>{mercado.taxaAcerto.toFixed(2)}%</td>
                        <td className={mercado.roi >= 0 ? 'positive' : 'negative'}>
                          {mercado.roi.toFixed(2)}%
                        </td>
                        <td className={mercado.lucro >= 0 ? 'positive' : 'negative'}>
                          R$ {mercado.lucro.toFixed(2)}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Observações */}
          {fechamento.observacoes && (
            <div className="closing-notes">
              <h3>Observações</h3>
              <p>{fechamento.observacoes}</p>
            </div>
          )}
        </div>
      )}

      {!loading && !fechamento && !error && (
        <div className="no-data">
          <p>Selecione um mês e ano para visualizar o fechamento mensal.</p>
        </div>
      )}
    </div>
  );
}

export default MonthlyClosing;

