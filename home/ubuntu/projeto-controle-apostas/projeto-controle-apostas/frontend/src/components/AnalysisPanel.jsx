import { useState, useEffect } from 'react';
import { analysisService } from '../services/api';
import '../styles/Analysis.css';

function AnalysisPanel() {
  const [roi, setRoi] = useState(0);
  const [yield_, setYield] = useState(0);
  const [taxaAcerto, setTaxaAcerto] = useState(0);
  const [lucroLiquido, setLucroLiquido] = useState(0);
  const [lucroAcumulado, setLucroAcumulado] = useState(0);
  const [mercados, setMercados] = useState([]);
  const [campeonatos, setCampeonatos] = useState([]);
  const [alertas, setAlertas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadAnalytics();
  }, []);

  const loadAnalytics = async () => {
    try {
      setLoading(true);
      setError('');

      const [
        roiRes,
        yieldRes,
        taxaRes,
        lucroRes,
        acumuladoRes,
        mercadosRes,
        campeonatosRes,
        alertasRes
      ] = await Promise.all([
        analysisService.getRoiGeral(),
        analysisService.getYield(),
        analysisService.getTaxaAcerto(),
        analysisService.getLucroLiquido(),
        analysisService.getLucroAcumulado(),
        analysisService.getAnaliseMercados(),
        analysisService.getAnaliseCampeonatos(),
        analysisService.getAlertasInteligentes()
      ]);

      setRoi(roiRes.data?.roi || 0);
      setYield(yieldRes.data?.yield || 0);
      setTaxaAcerto(taxaRes.data?.taxaAcerto || 0);
      setLucroLiquido(lucroRes.data?.lucroLiquido || 0);
      setLucroAcumulado(acumuladoRes.data?.lucroAcumulado || 0);
      setMercados(mercadosRes.data || []);
      setCampeonatos(campeonatosRes.data || []);
      setAlertas(alertasRes.data || []);
    } catch (err) {
      console.error('Erro ao carregar análises:', err);
      setError('Erro ao carregar análises. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="analysis-panel loading">Carregando análises...</div>;
  }

  return (
    <div className="analysis-panel">
      <h2>Análises e Indicadores</h2>

      {error && <div className="error-message">{error}</div>}

      {/* Indicadores Principais */}
      <div className="indicators-grid">
        <div className="indicator-card">
          <h3>ROI Geral</h3>
          <div className={`indicator-value ${roi >= 0 ? 'positive' : 'negative'}`}>
            {roi.toFixed(2)}%
          </div>
          <p className="indicator-label">Retorno sobre Investimento</p>
        </div>

        <div className="indicator-card">
          <h3>Yield</h3>
          <div className={`indicator-value ${yield_ >= 0 ? 'positive' : 'negative'}`}>
            {yield_.toFixed(2)}%
          </div>
          <p className="indicator-label">Rendimento Percentual</p>
        </div>

        <div className="indicator-card">
          <h3>Taxa de Acerto</h3>
          <div className={`indicator-value ${taxaAcerto >= 50 ? 'positive' : 'negative'}`}>
            {taxaAcerto.toFixed(2)}%
          </div>
          <p className="indicator-label">Porcentagem de Vitórias</p>
        </div>

        <div className="indicator-card">
          <h3>Lucro Líquido</h3>
          <div className={`indicator-value ${lucroLiquido >= 0 ? 'positive' : 'negative'}`}>
            R$ {lucroLiquido.toFixed(2)}
          </div>
          <p className="indicator-label">Ganho/Perda Total</p>
        </div>

        <div className="indicator-card">
          <h3>Lucro Acumulado</h3>
          <div className={`indicator-value ${lucroAcumulado >= 0 ? 'positive' : 'negative'}`}>
            R$ {lucroAcumulado.toFixed(2)}
          </div>
          <p className="indicator-label">Acumulado de Ciclos</p>
        </div>
      </div>

      {/* Análise de Mercados */}
      {mercados.length > 0 && (
        <div className="analysis-section">
          <h3>Análise por Mercado</h3>
          <div className="analysis-table">
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
                {mercados.map((m, idx) => (
                  <tr key={idx}>
                    <td>{m.mercado}</td>
                    <td>{m.totalApostas}</td>
                    <td>{m.apostasGanhas}</td>
                    <td>{m.taxaAcerto.toFixed(2)}%</td>
                    <td className={m.roi >= 0 ? 'positive' : 'negative'}>
                      {m.roi.toFixed(2)}%
                    </td>
                    <td className={m.lucro >= 0 ? 'positive' : 'negative'}>
                      R$ {m.lucro.toFixed(2)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Análise de Campeonatos */}
      {campeonatos.length > 0 && (
        <div className="analysis-section">
          <h3>Análise por Campeonato</h3>
          <div className="analysis-table">
            <table>
              <thead>
                <tr>
                  <th>Campeonato</th>
                  <th>Apostas</th>
                  <th>Ganhas</th>
                  <th>Taxa Acerto</th>
                  <th>ROI</th>
                  <th>Lucro</th>
                </tr>
              </thead>
              <tbody>
                {campeonatos.map((c, idx) => (
                  <tr key={idx}>
                    <td>{c.campeonato}</td>
                    <td>{c.totalApostas}</td>
                    <td>{c.apostasGanhas}</td>
                    <td>{c.taxaAcerto.toFixed(2)}%</td>
                    <td className={c.roi >= 0 ? 'positive' : 'negative'}>
                      {c.roi.toFixed(2)}%
                    </td>
                    <td className={c.lucro >= 0 ? 'positive' : 'negative'}>
                      R$ {c.lucro.toFixed(2)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Alertas Inteligentes */}
      {alertas.length > 0 && (
        <div className="analysis-section">
          <h3>Alertas Inteligentes</h3>
          <div className="alerts-list">
            {alertas.map((alerta, idx) => (
              <div key={idx} className={`alert alert-${alerta.tipo.toLowerCase()}`}>
                <strong>{alerta.titulo}</strong>
                <p>{alerta.mensagem}</p>
              </div>
            ))}
          </div>
        </div>
      )}

      <button onClick={loadAnalytics} className="btn-refresh">
        🔄 Atualizar Análises
      </button>
    </div>
  );
}

export default AnalysisPanel;

