import { useState } from 'react';
import { cicloService } from '../services/api';
import '../styles/Form.css';

function CicloForm({ onSuccess }) {
  const [name, setName] = useState('');
  const [startDate, setStartDate] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await cicloService.create(name, new Date(startDate).toISOString());
      setName('');
      setStartDate('');
      onSuccess();
    } catch (err) {
      setError(err.response?.data?.message || 'Erro ao criar ciclo.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="form-container">
      <div className="form-group">
        <label htmlFor="name">Nome do Ciclo:</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          placeholder="Ex: Ciclo de Outubro"
        />
      </div>
      <div className="form-group">
        <label htmlFor="startDate">Data de Início:</label>
        <input
          type="date"
          id="startDate"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
          required
        />
      </div>
      {error && <div className="error-message">{error}</div>}
      <button type="submit" disabled={loading} className="btn-primary">
        {loading ? 'Criando...' : 'Criar Ciclo'}
      </button>
    </form>
  );
}

export default CicloForm;

