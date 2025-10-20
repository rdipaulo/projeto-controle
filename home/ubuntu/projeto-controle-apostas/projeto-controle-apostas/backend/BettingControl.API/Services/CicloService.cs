using BettingControl.API.Dtos;
using BettingControl.API.Models;
using BettingControl.API.Repositories;

namespace BettingControl.API.Services
{
    public class CicloService : ICicloService
    {
        private readonly ICicloRepository _cicloRepository;

        public CicloService(ICicloRepository cicloRepository)
        {
            _cicloRepository = cicloRepository;
        }

        public Task<IEnumerable<Ciclo>> GetUserCiclosAsync(int userId)
        {
            return _cicloRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<Ciclo?> GetCicloByIdAsync(int id, int userId)
        {
            var ciclo = await _cicloRepository.GetByIdAsync(id);
            return ciclo?.UserId == userId ? ciclo : null;
        }

        public async Task<Ciclo> CreateCicloAsync(CreateCicloDto dto, int userId)
        {
            var ciclo = new Ciclo
            {
                UserId = userId,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = null // Começa sempre aberto, EndDate é opcional
            };

            await _cicloRepository.AddAsync(ciclo);
            return ciclo;
        }

        public async Task<bool> UpdateCicloAsync(int id, UpdateCicloDto dto, int userId)
        {
            var ciclo = await _cicloRepository.GetByIdAsync(id);

            if (ciclo == null || ciclo.UserId != userId)
            {
                return false;
            }

            ciclo.Name = dto.Name;
            ciclo.StartDate = dto.StartDate;
            ciclo.EndDate = dto.EndDate;

            await _cicloRepository.UpdateAsync(ciclo);
            return true;
        }

        public async Task<bool> DeleteCicloAsync(int id, int userId)
        {
            var ciclo = await _cicloRepository.GetByIdAsync(id);

            if (ciclo == null || ciclo.UserId != userId)
            {
                return false;
            }

            // Nota: Em um sistema real, você deve verificar se existem apostas (Bets)
            // associadas a este ciclo antes de deletar, ou usar deleção em cascata no EF.

            await _cicloRepository.DeleteAsync(ciclo);
            return true;
        }

        public async Task<CicloDto?> EncerrarCicloAsync(int id, int userId)
        {
            var ciclo = await _cicloRepository.GetByIdAsync(id);

            if (ciclo == null || ciclo.UserId != userId)
            {
                return null; // Ciclo não encontrado ou não pertence ao usuário
            }

            if (ciclo.IsClosed)
            {
                return null; // Ciclo já está encerrado (retorna null se já estiver encerrado para indicar que nenhuma ação foi tomada)
            }

            // Carregar as apostas associadas ao ciclo
            // Isso pode exigir uma alteração no CicloRepository para incluir as Bets
            // Por enquanto, vamos assumir que as Bets já estão carregadas ou que o GetByIdAsync as carrega
            // Se não, precisaremos ajustar o repositório.

            ciclo.TotalApostado = 0;
            ciclo.TotalGanhos = 0;
            ciclo.LucroPrejuizo = 0;

            foreach (var bet in ciclo.Bets)
            {
                ciclo.TotalApostado += bet.ValorApostado;
                if (bet.Resultado == ResultadoAposta.Ganha)
                {
                    ciclo.TotalGanhos += bet.ValorApostado * bet.Odd;
                }
                ciclo.LucroPrejuizo += bet.LucroPrejuizo;
            }

            // Calcular ROI
            if (ciclo.TotalApostado > 0)
            {
                ciclo.ROI = (ciclo.LucroPrejuizo / ciclo.TotalApostado) * 100;
            }
            else
            {
                ciclo.ROI = 0;
            }

            ciclo.EndDate = DateTime.UtcNow; // Define a data de encerramento
            ciclo.IsClosed = true; // Marca o ciclo como encerrado

            await _cicloRepository.UpdateAsync(ciclo);

            // Mapeia para CicloDto antes de retornar para evitar referência circular
            return new CicloDto
            {
                Id = ciclo.Id,
                Name = ciclo.Name,
                StartDate = ciclo.StartDate,
                EndDate = ciclo.EndDate,
                TotalApostado = ciclo.TotalApostado,
                TotalGanhos = ciclo.TotalGanhos,
                LucroPrejuizo = ciclo.LucroPrejuizo,
                Roi = ciclo.ROI,
                IsClosed = ciclo.IsClosed
            };
        }
    }
}