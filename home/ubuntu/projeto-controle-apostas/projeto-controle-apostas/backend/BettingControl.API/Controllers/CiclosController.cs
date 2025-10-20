using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BettingControl.API.Services;
using BettingControl.API.Dtos;
using System.Security.Claims;
using BettingControl.API.Models;

namespace BettingControl.API.Controllers
{
    [Authorize] // Protege todos os endpoints
    [Route("api/[controller]")]
    [ApiController]
    public class CiclosController : ControllerBase
    {
        private readonly ICicloService _cicloService;

        public CiclosController(ICicloService cicloService)
        {
            _cicloService = cicloService;
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

        // GET api/ciclos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ciclo>>> GetUserCiclos()
        {
            var userId = GetUserId();
            var ciclos = await _cicloService.GetUserCiclosAsync(userId);
            return Ok(ciclos);
        }

        // GET api/ciclos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ciclo>> GetCiclo(int id)
        {
            var userId = GetUserId();
            var ciclo = await _cicloService.GetCicloByIdAsync(id, userId);

            if (ciclo == null)
            {
                return NotFound("Ciclo não encontrado ou acesso negado.");
            }

            return Ok(ciclo);
        }

        // POST api/ciclos
        [HttpPost]
        public async Task<ActionResult<Ciclo>> PostCiclo([FromBody] CreateCicloDto dto)
        {
            var userId = GetUserId();
            var novoCiclo = await _cicloService.CreateCicloAsync(dto, userId);
            
            return CreatedAtAction(nameof(GetCiclo), new { id = novoCiclo.Id }, novoCiclo);
        }

        // PUT api/ciclos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiclo(int id, [FromBody] UpdateCicloDto dto)
        {
            var userId = GetUserId();
            var success = await _cicloService.UpdateCicloAsync(id, dto, userId);

            if (!success)
            {
                return NotFound("Ciclo não encontrado ou acesso negado.");
            }

            return NoContent();
        }

        // DELETE api/ciclos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiclo(int id)
        {
            var userId = GetUserId();
            var success = await _cicloService.DeleteCicloAsync(id, userId);

            if (!success)
            {
                return NotFound("Ciclo não encontrado ou acesso negado.");
            }

            return NoContent();
        }

        // PUT api/ciclos/encerrar/{id}
        [HttpPut("encerrar/{id}")]
        public async Task<ActionResult<CicloDto>> EncerrarCiclo(int id)
        {
            var userId = GetUserId();
            var cicloEncerrado = await _cicloService.EncerrarCicloAsync(id, userId);

            if (cicloEncerrado == null)
            {
                return NotFound("Ciclo não encontrado, acesso negado ou já encerrado.");
            }

            return Ok(cicloEncerrado);
        }
    }
}