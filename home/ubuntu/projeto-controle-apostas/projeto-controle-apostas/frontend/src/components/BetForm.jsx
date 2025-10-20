import { useState } from 'react';
import { betService } from '../services/api';
import '../styles/Form.css';

function BetForm({ cicloId, onSuccess }) {
  const [tipoEsporte, setTipoEsporte] = useState('0'); // 0 = Futebol, 1 = Basquete
  const [pais, setPais] = useState('');
  const [continente, setContinente] = useState('');
  const [campeonato, setCampeonato] = useState('');
  const [timeCasa, setTimeCasa] = useState('');
  const [timeVisitante, setTimeVisitante] = useState('');
  const [mercado, setMercado] = useState('');
  const [odd, setOdd] = useState('');
  const [valorApostado, setValorApostado] = useState('');
  const [resultado, setResultado] = useState('0'); // 0 = Pendente, 1 = Ganhou, 2 = Perdeu
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  // Sugestões de mercados por esporte
  const mercadosSugeridos = {
    '0': ['Over 2.5', 'Under 2.5', 'Ambos Marcam', 'Resultado 1X2', 'Handicap', 'Empate'],
    '1': ['Over 200.5', 'Under 200.5', 'Spread', 'Total de Pontos', 'Vencedor', 'Placar Exato']
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const bet = {
        cicloId: parseInt(cicloId),
        tipoEsporte: parseInt(tipoEsporte),
        pais,
        continente,
        campeonato,
        timeCasa,
        timeVisitante,
        mercado,
        odd: parseFloat(odd),
        valorApostado: parseFloat(valorApostado),
        resultado: parseInt(resultado),
        data: new Date().toISOString(),
      };

      await betService.create(bet);
      
      // Limpar formulário
      setTipoEsporte('0');
      setPais('');
      setContinente('');
      setCampeonato('');
      setTimeCasa('');
      setTimeVisitante('');
      setMercado('');
      setOdd('');
      setValorApostado('');
      setResultado('0');
      
      onSuccess();
    } catch (err) {
      setError(err.response?.data?.message || 'Erro ao criar aposta.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="form-container bet-form">
      <h3>Nova Aposta</h3>
      
      {/* Seletor de Esporte com Imagens */}
      <div className="sport-selector">
        <div className="form-group">
          <label>Tipo de Esporte:</label>
          <div className="sport-options">
            <div 
              className={`sport-option ${tipoEsporte === '0' ? 'active' : ''}`}
              onClick={() => setTipoEsporte('0')}
            >
              <img src="/images/football-field.jpg" alt="Futebol" />
              <span>⚽ Futebol</span>
            </div>
            <div 
              className={`sport-option ${tipoEsporte === '1' ? 'active' : ''}`}
              onClick={() => setTipoEsporte('1')}
            >
              <img src="/images/basketball-court.jpg" alt="Basquete" />
              <span>🏀 Basquete</span>
            </div>
          </div>
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="pais">País:</label>
        <input
          type="text"
          id="pais"
          value={pais}
          onChange={(e) => setPais(e.target.value)}
          required
          placeholder={tipoEsporte === '0' ? "Ex: Brasil" : "Ex: EUA"}
        />
      </div>

      <div className="form-group">
        <label htmlFor="continente">Continente:</label>
        <input
          type="text"
          id="continente"
          value={continente}
          onChange={(e) => setContinente(e.target.value)}
          required
          placeholder={tipoEsporte === '0' ? "Ex: América do Sul" : "Ex: América do Norte"}
        />
      </div>

      <div className="form-group">
        <label htmlFor="campeonato">Campeonato:</label>
        <input
          type="text"
          id="campeonato"
          value={campeonato}
          onChange={(e) => setCampeonato(e.target.value)}
          required
          placeholder={tipoEsporte === '0' ? "Ex: Brasileirão" : "Ex: NBA"}
        />
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="timeCasa">{tipoEsporte === '0' ? 'Time Casa' : 'Jogador/Time Casa'}:</label>
          <input
            type="text"
            id="timeCasa"
            value={timeCasa}
            onChange={(e) => setTimeCasa(e.target.value)}
            required
            placeholder={tipoEsporte === '0' ? "Ex: Flamengo" : "Ex: Los Angeles Lakers"}
          />
        </div>

        <div className="form-group">
          <label htmlFor="timeVisitante">{tipoEsporte === '0' ? 'Time Visitante' : 'Jogador/Time Visitante'}:</label>
          <input
            type="text"
            id="timeVisitante"
            value={timeVisitante}
            onChange={(e) => setTimeVisitante(e.target.value)}
            required
            placeholder={tipoEsporte === '0' ? "Ex: Vasco" : "Ex: Boston Celtics"}
          />
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="mercado">Mercado:</label>
        <input
          type="text"
          id="mercado"
          value={mercado}
          onChange={(e) => setMercado(e.target.value)}
          required
          placeholder={tipoEsporte === '0' ? "Ex: Over 2.5, Ambos Marcam" : "Ex: Over 200.5, Spread"}
          list="mercados-list"
        />
        <datalist id="mercados-list">
          {mercadosSugeridos[tipoEsporte].map((m) => (
            <option key={m} value={m} />
          ))}
        </datalist>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="odd">Odd:</label>
          <input
            type="number"
            id="odd"
            value={odd}
            onChange={(e) => setOdd(e.target.value)}
            required
            step="0.01"
            min="1"
            placeholder="Ex: 1.80"
          />
        </div>

        <div className="form-group">
          <label htmlFor="valorApostado">Valor Apostado (R$):</label>
          <input
            type="number"
            id="valorApostado"
            value={valorApostado}
            onChange={(e) => setValorApostado(e.target.value)}
            required
            step="0.01"
            min="0.01"
            placeholder="Ex: 100.00"
          />
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="resultado">Resultado:</label>
        <select
          id="resultado"
          value={resultado}
          onChange={(e) => setResultado(e.target.value)}
          required
        >
          <option value="0">Pendente</option>
          <option value="1">Ganhou</option>
          <option value="2">Perdeu</option>
        </select>
      </div>

      {error && <div className="error-message">{error}</div>}
      
      <button type="submit" disabled={loading} className="btn-primary">
        {loading ? 'Criando...' : 'Criar Aposta'}
      </button>
    </form>
  );
}

export default BetForm;

