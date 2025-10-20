using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BettingControl.API.Data;
using BettingControl.API.Dtos;
using BettingControl.API.Models;
using BettingControl.API.Repositories;

namespace BettingControl.API.Services
{
    public class BetService : IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly ApplicationDbContext _context; // Para acessar Ciclos diretamente, se necessário

        public BetService(IBetRepository betRepository, ApplicationDbContext context)
        {
            _betRepository = betRepository;
            _context = context;
        }

        public async Task<IEnumerable<Bet>> GetAllBetsAsync()
        {
            return await _betRepository.GetAllAsync();
        }

        public async Task<Bet> GetBetByIdAsync(int id)
        {
            return await _betRepository.GetByIdAsync(id);
        }

        public async Task<Bet> AddBetAsync(CreateBetDto createBetDto)
        {
            var bet = new Bet
            {
                Pais = createBetDto.Pais,
                Continente = createBetDto.Continente,
                Campeonato = createBetDto.Campeonato,
                TimeCasa = createBetDto.TimeCasa,
                TimeVisitante = createBetDto.TimeVisitante,
                Mercado = createBetDto.Mercado,
                Odd = createBetDto.Odd,
                ValorApostado = createBetDto.ValorApostado,
                Resultado = ResultadoAposta.Pendente, // Nova aposta sempre começa como Pendente
                DataAposta = DateTime.UtcNow,
                CicloId = createBetDto.CicloId
            };

            // O LucroPrejuizo é calculado apenas quando o resultado não é Pendente. Por enquanto, 0.
            bet.LucroPrejuizo = 0;

            await _betRepository.AddAsync(bet);
            return bet;
        }

        public async Task<Bet> UpdateBetAsync(int id, UpdateBetDto updateBetDto)
        {
            var betExistente = await _betRepository.GetByIdAsync(id);
            if (betExistente == null)
            {
                return null;
            }

            betExistente.Pais = updateBetDto.Pais;
            betExistente.Continente = updateBetDto.Continente;
            betExistente.Campeonato = updateBetDto.Campeonato;
            betExistente.TimeCasa = updateBetDto.TimeCasa;
            betExistente.TimeVisitante = updateBetDto.TimeVisitante;
            betExistente.Mercado = updateBetDto.Mercado;
            betExistente.Odd = updateBetDto.Odd;
            betExistente.ValorApostado = updateBetDto.ValorApostado;
            betExistente.CicloId = updateBetDto.CicloId;

            // Atualiza o resultado e calcula LucroPrejuizo se o resultado mudou
            if (betExistente.Resultado != updateBetDto.Resultado)
            {
                betExistente.Resultado = updateBetDto.Resultado;
                betExistente.LucroPrejuizo = CalcularLucroPrejuizo(betExistente.Resultado, betExistente.ValorApostado, betExistente.Odd);
            }

            await _betRepository.UpdateAsync(betExistente);
            return betExistente;
        }

        public async Task<bool> DeleteBetAsync(int id)
        {
            var betExistente = await _betRepository.GetByIdAsync(id);
            if (betExistente == null)
            {
                return false;
            }

            await _betRepository.DeleteAsync(betExistente);
            return true;
        }

        private decimal CalcularLucroPrejuizo(ResultadoAposta resultado, decimal valorApostado, decimal odd)
        {
            return resultado switch
            {
                ResultadoAposta.Ganha => valorApostado * (odd - 1),
                ResultadoAposta.Perdida => -valorApostado,
                _ => 0m, // Pendente, Cancelada ou outros não geram lucro/prejuízo imediato
            };
        }
    }
}

