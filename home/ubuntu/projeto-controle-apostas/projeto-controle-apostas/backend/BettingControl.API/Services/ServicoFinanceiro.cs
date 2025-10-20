using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingControl.API.Data;
using BettingControl.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BettingControl.API.Services
{
    public class ServicoFinanceiro : IServicoFinanceiro
    {
        private readonly ApplicationDbContext _context;

        public ServicoFinanceiro(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalcularROIGeralAsync(int userId)
        {
            var bets = await _context.Bets
                .Where(b => b.Ciclo.UserId == userId && b.Resultado != ResultadoAposta.Pendente)
                .ToListAsync();

            var totalApostado = bets.Sum(b => b.ValorApostado);
            var lucroPrejuizoTotal = bets.Sum(b => b.LucroPrejuizo);

            if (totalApostado == 0) return 0;

            return (lucroPrejuizoTotal / totalApostado) * 100;
        }

        public async Task<decimal> CalcularROICicloAsync(int cicloId, int userId)
        {
            var ciclo = await _context.Ciclos
                .Include(c => c.Bets)
                .FirstOrDefaultAsync(c => c.Id == cicloId && c.UserId == userId);

            if (ciclo == null) return 0;

            var betsDoCiclo = ciclo.Bets.Where(b => b.Resultado != ResultadoAposta.Pendente).ToList();

            var totalApostado = betsDoCiclo.Sum(b => b.ValorApostado);
            var lucroPrejuizoTotal = betsDoCiclo.Sum(b => b.LucroPrejuizo);

            if (totalApostado == 0) return 0;

            return (lucroPrejuizoTotal / totalApostado) * 100;
        }

        public async Task<decimal> CalcularYieldAsync(int userId)
        {
            var bets = await _context.Bets
                .Where(b => b.Ciclo.UserId == userId && b.Resultado != ResultadoAposta.Pendente)
                .ToListAsync();

            var totalApostado = bets.Sum(b => b.ValorApostado);
            var lucroPrejuizoTotal = bets.Sum(b => b.LucroPrejuizo);

            if (totalApostado == 0) return 0;

            return (lucroPrejuizoTotal / totalApostado) * 100; // Yield é o mesmo cálculo do ROI, mas o termo é mais usado em apostas
        }

        public async Task<decimal> CalcularTaxaAcertoAsync(int userId)
        {
            var bets = await _context.Bets
                .Where(b => b.Ciclo.UserId == userId && b.Resultado != ResultadoAposta.Pendente)
                .ToListAsync();

            var totalApostasResolvidas = bets.Count();
            var apostasGanhas = bets.Count(b => b.Resultado == ResultadoAposta.Ganha);

            if (totalApostasResolvidas == 0) return 0;

            return ((decimal)apostasGanhas / totalApostasResolvidas) * 100;
        }

        public async Task<decimal> CalcularLucroLiquidoAsync(int userId)
        {
            var bets = await _context.Bets
                .Where(b => b.Ciclo.UserId == userId && b.Resultado != ResultadoAposta.Pendente)
                .ToListAsync();

            return bets.Sum(b => b.LucroPrejuizo);
        }

        public async Task<decimal> CalcularLucroAcumuladoAsync(int userId)
        {
            // O lucro acumulado é o lucro líquido atual mais o saldo inicial da banca.
            // Para simplificar, vamos considerar o lucro líquido como o acumulado por enquanto.
            // Em um cenário real, precisaríamos de um registro de saldo inicial ou de depósitos/saques.
            return await CalcularLucroLiquidoAsync(userId);
        }

        public async Task RegistrarHistoricoBancaAsync(int userId, decimal saldo, string observacao)
        {
            var historico = new HistoricoBanca
            {
                UserId = userId,
                DataRegistro = DateTime.UtcNow,
                Saldo = saldo,
                Observacao = observacao
            };
            await _context.HistoricoBancas.AddAsync(historico);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HistoricoBanca>> GetHistoricoBancaAsync(int userId)
        {
            return await _context.HistoricoBancas
                .Where(h => h.UserId == userId)
                .OrderBy(h => h.DataRegistro)
                .ToListAsync();
        }
    }
}

