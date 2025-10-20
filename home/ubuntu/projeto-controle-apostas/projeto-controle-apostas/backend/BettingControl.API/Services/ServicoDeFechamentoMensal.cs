using System;
using System.Linq;
using System.Threading.Tasks;
using BettingControl.API.Data;
using BettingControl.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BettingControl.API.Services
{
    public class ServicoDeFechamentoMensal : IServicoDeFechamentoMensal
    {
        private readonly ApplicationDbContext _context;
        private readonly IServicoFinanceiro _servicoFinanceiro;
        private readonly IServicoDeAnalise _servicoDeAnalise;

        public ServicoDeFechamentoMensal(
            ApplicationDbContext context,
            IServicoFinanceiro servicoFinanceiro,
            IServicoDeAnalise servicoDeAnalise)
        {
            _context = context;
            _servicoFinanceiro = servicoFinanceiro;
            _servicoDeAnalise = servicoDeAnalise;
        }

        public async Task<FechamentoMensal> RealizarFechamentoMensalAsync(int userId, int ano, int mes)
        {
            var mesReferencia = new DateTime(ano, mes, 1);
            var proximoMes = mesReferencia.AddMonths(1);

            // Verificar se já existe um fechamento para este mês
            var fechamentoExistente = await _context.FechamentosMensais
                .FirstOrDefaultAsync(fm => fm.UserId == userId && fm.MesReferencia == mesReferencia);

            if (fechamentoExistente != null)
            {
                // Se já existe, podemos atualizar ou retornar o existente
                // Por simplicidade, vamos retornar o existente por enquanto.
                return fechamentoExistente;
            }

            // Obter todas as apostas do usuário para o mês de referência
            var betsDoMes = await _context.Bets
                .Include(b => b.Ciclo)
                .Where(b => b.Ciclo.UserId == userId &&
                            b.DataAposta >= mesReferencia &&
                            b.DataAposta < proximoMes &&
                            b.Resultado != ResultadoAposta.Pendente)
                .ToListAsync();

            var totalApostado = betsDoMes.Sum(b => b.ValorApostado);
            var lucroPrejuizo = betsDoMes.Sum(b => b.LucroPrejuizo);
            var totalApostasResolvidas = betsDoMes.Count();
            var apostasGanhas = betsDoMes.Count(b => b.Resultado == ResultadoAposta.Ganha);

            decimal roi = 0;
            decimal yield = 0;
            decimal taxaAcerto = 0;

            if (totalApostado > 0)
            {
                roi = (lucroPrejuizo / totalApostado) * 100;
                yield = roi; // Yield é o mesmo cálculo do ROI neste contexto
            }
            if (totalApostasResolvidas > 0)
            {
                taxaAcerto = ((decimal)apostasGanhas / totalApostasResolvidas) * 100;
            }

            // Obter sugestões de análise
            var sugestoesAnalise = await _servicoDeAnalise.EmitirAlertasInteligentesAsync(userId);
            var sugestoesString = string.Join("; ", sugestoesAnalise);

            var novoFechamento = new FechamentoMensal
            {
                UserId = userId,
                MesReferencia = mesReferencia,
                TotalApostado = totalApostado,
                TotalGanhos = betsDoMes.Where(b => b.Resultado == ResultadoAposta.Ganha).Sum(b => b.ValorApostado * b.Odd),
                LucroPrejuizo = lucroPrejuizo,
                ROI = roi,
                Yield = yield,
                TaxaAcerto = taxaAcerto,
                SugestoesAnalise = sugestoesString
            };

            await _context.FechamentosMensais.AddAsync(novoFechamento);
            await _context.SaveChangesAsync();

            return novoFechamento;
        }

        public async Task<FechamentoMensal> GetFechamentoMensalAsync(int userId, int ano, int mes)
        {
            var mesReferencia = new DateTime(ano, mes, 1);

            return await _context.FechamentosMensais
                .FirstOrDefaultAsync(fm => fm.UserId == userId && fm.MesReferencia == mesReferencia);
        }
    }
}

