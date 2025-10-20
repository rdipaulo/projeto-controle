import { cicloService } from '../services/api';
import '../styles/CicloList.css';

function CicloList({ ciclos, selectedCiclo, onSelect, onRefresh }) {
  const handleEncerrarCiclo = async (cicloId) => {
    if (window.confirm('Tem certeza que deseja encerrar este ciclo?')) {
      try {
        await cicloService.encerrar(cicloId);
        onRefresh();
      } catch (err) {
        alert('Erro ao encerrar ciclo: ' + (err.response?.data?.message || err.message));
      }
    }
  };

  const handleDeleteCiclo = async (cicloId) => {
    if (window.confirm('Tem certeza que deseja deletar este ciclo?')) {
      try {
        await cicloService.delete(cicloId);
        onRefresh();
      } catch (err) {
        alert('Erro ao deletar ciclo: ' + (err.response?.data?.message || err.message));
      }
    }
  };

  return (
    <div className="ciclo-list">
      {ciclos.map((ciclo) => (
        <div
          key={ciclo.id}
          className={`ciclo-item ${selectedCiclo?.id === ciclo.id ? 'selected' : ''}`}
          onClick={() => onSelect(ciclo)}
        >
          <h3>{ciclo.name}</h3>
          <p className="ciclo-status">
            {ciclo.isClosed ? '✓ Encerrado' : '● Aberto'}
          </p>
          <div className="ciclo-actions">
            {!ciclo.isClosed && (
              <button
                onClick={(e) => {
                  e.stopPropagation();
                  handleEncerrarCiclo(ciclo.id);
                }}
                className="btn-small btn-encerrar"
              >
                Encerrar
              </button>
            )}
            <button
              onClick={(e) => {
                e.stopPropagation();
                handleDeleteCiclo(ciclo.id);
              }}
              className="btn-small btn-delete"
            >
              Deletar
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}

export default CicloList;

