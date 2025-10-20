using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BettingControl.API.Models;
using BettingControl.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BettingControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnalisesController : ControllerBase
    {
        private readonly IServicoFinanceiro _servicoFinanceiro;
        private readonly IServicoDeAnalise _servicoDeAnalise;
        private readonly IServicoDeFechamentoMensal _servicoDeFechamentoMensal;

        public AnalisesController(IServicoFinanceiro servicoFinanceiro, IServicoDeAnalise servicoDeAnalise, IServicoDeFechamentoMensal servicoDeFechamentoMensal)
        {
            _servicoFinanceiro = servicoFinanceiro;
            _servicoDeAnalise = servicoDeAnalise;
            _servicoDeFechamentoMensal = servicoDeFechamentoMensal;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("ID do usuário não encontrado no token.");
        }

        // GET: api/Analises/roi-geral
        [HttpGet("roi-geral")]
        public async Task<ActionResult<decimal>> GetROIGeral()
        {
            var userId = GetUserId();
            var roi = await _servicoFinanceiro.CalcularROIGeralAsync(userId);
            return Ok(roi);
        }

        // GET: api/Analises/roi-ciclo/{cicloId}
        [HttpGet("roi-ciclo/{cicloId}")]
        public async Task<ActionResult<decimal>> GetROICiclo(int cicloId)
        {
            var userId = GetUserId();
            var roi = await _servicoFinanceiro.CalcularROICicloAsync(cicloId, userId);
            return Ok(roi);
        }

        // GET: api/Analises/yield
        [HttpGet("yield")]
        public async Task<ActionResult<decimal>> GetYield()
        {
            var userId = GetUserId();
            var yield = await _servicoFinanceiro.CalcularYieldAsync(userId);
            return Ok(yield);
        }

        // GET: api/Analises/taxa-acerto
        [HttpGet("taxa-acerto")]
        public async Task<ActionResult<decimal>> GetTaxaAcerto()
        {
            var userId = GetUserId();
            var taxaAcerto = await _servicoFinanceiro.CalcularTaxaAcertoAsync(userId);
            return Ok(taxaAcerto);
        }

        // GET: api/Analises/lucro-liquido
        [HttpGet("lucro-liquido")]
        public async Task<ActionResult<decimal>> GetLucroLiquido()
        {
            var userId = GetUserId();
            var lucroLiquido = await _servicoFinanceiro.CalcularLucroLiquidoAsync(userId);
            return Ok(lucroLiquido);
        }

        // GET: api/Analises/lucro-acumulado
        [HttpGet("lucro-acumulado")]
        public async Task<ActionResult<decimal>> GetLucroAcumulado()
        {
            var userId = GetUserId();
            var lucroAcumulado = await _servicoFinanceiro.CalcularLucroAcumuladoAsync(userId);
            return Ok(lucroAcumulado);
        }

        // GET: api/Analises/historico-banca
        [HttpGet("historico-banca")]
        public async Task<ActionResult<IEnumerable<HistoricoBanca>>> GetHistoricoBanca()
        {
            var userId = GetUserId();
            var historico = await _servicoFinanceiro.GetHistoricoBancaAsync(userId);
            return Ok(historico);
        }

        // GET: api/Analises/mercados-lucrativos-deficitarios
        [HttpGet("mercados-lucrativos-deficitarios")]
        public async Task<ActionResult<IEnumerable<string>>> GetMercadosLucrativosDeficitarios()
        {
            var userId = GetUserId();
            var analise = await _servicoDeAnalise.AnalisarMercadosLucrativosDeficitariosAsync(userId);
            return Ok(analise);
        }

        // GET: api/Analises/campeonatos-lucrativos-deficitarios
        [HttpGet("campeonatos-lucrativos-deficitarios")]
        public async Task<ActionResult<IEnumerable<string>>> GetCampeonatosLucrativosDeficitarios()
        {
            var userId = GetUserId();
            var analise = await _servicoDeAnalise.AnalisarCampeonatosLucrativosDeficitariosAsync(userId);
            return Ok(analise);
        }

        // GET: api/Analises/paises-lucrativos-deficitarios
        [HttpGet("paises-lucrativos-deficitarios")]
        public async Task<ActionResult<IEnumerable<string>>> GetPaisesLucrativosDeficitarios()
        {
            var userId = GetUserId();
            var analise = await _servicoDeAnalise.AnalisarPaisesLucrativosDeficitariosAsync(userId);
            return Ok(analise);
        }

        // GET: api/Analises/times-lucrativos-deficitarios
        [HttpGet("times-lucrativos-deficitarios")]
        public async Task<ActionResult<IEnumerable<string>>> GetTimesLucrativosDeficitarios()
        {
            var userId = GetUserId();
            var analise = await _servicoDeAnalise.AnalisarTimesLucrativosDeficitariosAsync(userId);
            return Ok(analise);
        }

        // GET: api/Analises/alertas-inteligentes
        [HttpGet("alertas-inteligentes")]
        public async Task<ActionResult<IEnumerable<string>>> GetAlertasInteligentes()
        {
            var userId = GetUserId();
            var alertas = await _servicoDeAnalise.EmitirAlertasInteligentesAsync(userId);
            return Ok(alertas);
        }

        // GET: api/Analises/fechamento-mensal/{ano}/{mes}
        [HttpGet("fechamento-mensal/{ano}/{mes}")]
        public async Task<ActionResult<FechamentoMensal>> GetFechamentoMensal(int ano, int mes)
        {
            var userId = GetUserId();
            var fechamento = await _servicoDeFechamentoMensal.GetFechamentoMensalAsync(userId, ano, mes);
            if (fechamento == null)
            {
                return NotFound("Fechamento mensal não encontrado.");
            }
            return Ok(fechamento);
        }

        // POST: api/Analises/realizar-fechamento-mensal/{ano}/{mes}
        [HttpPost("realizar-fechamento-mensal/{ano}/{mes}")]
        public async Task<ActionResult<FechamentoMensal>> RealizarFechamentoMensal(int ano, int mes)
        {
            var userId = GetUserId();
            var fechamento = await _servicoDeFechamentoMensal.RealizarFechamentoMensalAsync(userId, ano, mes);
            return CreatedAtAction(nameof(GetFechamentoMensal), new { ano = fechamento.MesReferencia.Year, mes = fechamento.MesReferencia.Month }, fechamento);
        }
    }
}

