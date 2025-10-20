using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BettingControl.API.Dtos;
using BettingControl.API.Models;
using BettingControl.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace BettingControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetsController(IBetService betService)
        {
            _betService = betService;
        }

        // GET: api/Apostas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bet>>> GetBets()
        {
            var bets = await _betService.GetAllBetsAsync();
            return Ok(bets);
        }

        // GET: api/Apostas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bet>> GetBet(int id)
        {
            var bet = await _betService.GetBetByIdAsync(id);

            if (bet == null)
            {
                return NotFound();
            }

            return Ok(bet);
        }

        // POST: api/Apostas
        [HttpPost]
      public async Task<ActionResult<Bet>> PostBet(CreateBetDto createBetDto)        {
            var bet = await _betService.AddBetAsync(createBetDto);
          return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);        }

        // PUT: api/Apostas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBet(int id, UpdateBetDto updateBetDto)
        {
            var betAtualizada = await _betService.UpdateBetAsync(id, updateBetDto);

            if (betAtualizada == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Apostas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBet(int id)
        {
            var result = await _betService.DeleteBetAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

