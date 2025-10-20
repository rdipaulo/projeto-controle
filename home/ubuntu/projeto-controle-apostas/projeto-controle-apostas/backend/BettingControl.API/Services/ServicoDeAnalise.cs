using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingControl.API.Data;
using BettingControl.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BettingControl.API.Services
{
    public class ServicoDeAnalise : IServicoDeAnalise
    {
        private readonly ApplicationDbContext _context;

        public ServicoDeAnalise(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<AnaliseMercadoDto>> GetAnalisePorCategoriaAsync(int userId, Func<Bet, string> categoriaSelector)
        {
            var bets = await _context.Bets
                .Include(b => b.Ciclo)
                .Where(b => b.Ciclo.UserId == userId && b.Resultado != ResultadoAposta.Pendente)
                .ToListAsync();

            var analisePorCategoria = bets.GroupBy(categoriaSelector)
                .Select(g => new AnaliseMercadoDto
                {
                    Nome = g.Key,
                    TotalApostas = g.Count(),
                    ApostasGanhas = g.Count(b => b.Resultado == ResultadoAposta.Ganha),
                    ApostasPerdidas = g.Count(b => b.Resultado == ResultadoAposta.Perdida),
                    LucroPrejuizo = g.Sum(b => b.LucroPrejuizo),
                    ROI = g.Sum(b => b.ValorApostado) > 0 ? (g.Sum(b => b.LucroPrejuizo) / g.Sum(b => b.ValorApostado)) * 100 : 0
                })
                .OrderByDescending(x => x.ROI)
                .ToList();

            return analisePorCategoria;
        }

        public async Task<IEnumerable<string>> AnalisarMercadosLucrativosDeficitariosAsync(int userId)
        {
            var analise = await GetAnalisePorCategoriaAsync(userId, b => b.Mercado);
            var resultados = new List<string>();
            resultados.Add("Mercados Mais Lucrativos:");
            analise.Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            resultados.Add("Mercados Mais Deficitários:");
            analise.OrderBy(a => a.ROI).Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            return resultados;
        }

        public async Task<IEnumerable<string>> AnalisarCampeonatosLucrativosDeficitariosAsync(int userId)
        {
            var analise = await GetAnalisePorCategoriaAsync(userId, b => b.Campeonato);
            var resultados = new List<string>();
            resultados.Add("Campeonatos Mais Lucrativos:");
            analise.Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            resultados.Add("Campeonatos Mais Deficitários:");
            analise.OrderBy(a => a.ROI).Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            return resultados;
        }

        public async Task<IEnumerable<string>> AnalisarPaisesLucrativosDeficitariosAsync(int userId)
        {
            var analise = await GetAnalisePorCategoriaAsync(userId, b => b.Pais);
            var resultados = new List<string>();
            resultados.Add("Países Mais Lucrativos:");
            analise.Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            resultados.Add("Países Mais Deficitários:");
            analise.OrderBy(a => a.ROI).Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            return resultados;
        }

        public async Task<IEnumerable<string>> AnalisarTimesLucrativosDeficitariosAsync(int userId)
        {
            var analiseCasa = await GetAnalisePorCategoriaAsync(userId, b => b.TimeCasa);
            var analiseVisitante = await GetAnalisePorCategoriaAsync(userId, b => b.TimeVisitante);
            var analiseCombinada = analiseCasa.Concat(analiseVisitante).GroupBy(a => a.Nome)
                .Select(g => new AnaliseMercadoDto
                {
                    Nome = g.Key,
                    TotalApostas = g.Sum(x => x.TotalApostas),
                    ApostasGanhas = g.Sum(x => x.ApostasGanhas),
                    ApostasPerdidas = g.Sum(x => x.ApostasPerdidas),
                    LucroPrejuizo = g.Sum(x => x.LucroPrejuizo),
                    ROI = g.Sum(x => x.TotalApostado) > 0 ? (g.Sum(x => x.LucroPrejuizo) / g.Sum(x => x.TotalApostado)) * 100 : 0
                })
                .OrderByDescending(x => x.ROI)
                .ToList();

            var resultados = new List<string>();
            resultados.Add("Times Mais Lucrativos:");
            analiseCombinada.Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            resultados.Add("Times Mais Deficitários:");
            analiseCombinada.OrderBy(a => a.ROI).Take(3).ToList().ForEach(a => resultados.Add($"- {a.Nome}: ROI {a.ROI:N2}%, Lucro {a.LucroPrejuizo:N2}"));
            return resultados;
        }

        public async Task<IEnumerable<string>> EmitirAlertasInteligentesAsync(int userId)
        {
            var alerts = new List<string>();
            var bets = await _context.Bets
                .Include(b => b.Ciclo)
                .Where(b => b.Ciclo.UserId == userId && b.Resultado != ResultadoAposta.Pendente)
                .OrderByDescending(b => b.DataAposta)
                .ToListAsync();

            // Alerta: Sequência de perdas em um mercado específico
            var mercadosComPerdas = bets.GroupBy(b => b.Mercado)
                .Where(g => g.Take(5).Count(b => b.Resultado == ResultadoAposta.Perdida) >= 3)
                .Select(g => g.Key)
                .ToList();

            foreach (var mercado in mercadosComPerdas)
            {
                alerts.Add($"ALERTA: O mercado '{mercado}' teve 3 ou mais perdas nas últimas 5 apostas. Considere revisar sua estratégia.");
            }

            // Alerta: Campeonato com ROI elevado
            var campeonatosComAltoROI = bets.GroupBy(b => b.Campeonato)
                .Select(g => new { Campeonato = g.Key, ROI = g.Sum(b => b.ValorApostado) > 0 ? (g.Sum(b => b.LucroPrejuizo) / g.Sum(b => b.ValorApostado)) * 100 : 0 })
                .Where(x => x.ROI > 20)
                .ToList();

            foreach (var camp in campeonatosComAltoROI)
            {
                alerts.Add($"SUGESTÃO: O campeonato '{camp.Campeonato}' apresenta um ROI de {camp.ROI:N2}%. Pode ser uma boa oportunidade!");
            }

            if (!alerts.Any())
            {
                alerts.Add("Nenhum alerta inteligente gerado no momento.");
            }

            return alerts;
        }

        public async Task<IEnumerable<AnaliseMercadoDto>> GetDadosAnaliseMercadoAsync(int userId)
        {
            return await GetAnalisePorCategoriaAsync(userId, b => b.Mercado);
        }
    }
}

