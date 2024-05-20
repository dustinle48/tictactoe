using Domain.Contracts.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TicTacToe.Server.Models;

namespace TicTacToe.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseGameDto[]>> GetAll(int? xPlayerId, int? oPlayerId, string? winner,
            CancellationToken cancellationToken)
        {
            var gameQuery = _gameRepository.GetAll();

            if (xPlayerId != null)
            {
                gameQuery = gameQuery.Where(g => g.XPlayerId == xPlayerId);
            }

            if (oPlayerId != null)
            {
                gameQuery = gameQuery.Where(g => g.OPlayerId == oPlayerId);
            }

            if (winner != null)
            {
                gameQuery = gameQuery.Where(g => g.Winner == winner);
            }

            var games = await gameQuery.ToArrayAsync(cancellationToken);

            var result = games
                .Select(g => new ResponseGameDto
                {
                    Id = g.Id,
                    XPlayerId = g.XPlayerId,
                    OPlayerId = g.OPlayerId,
                    Step = g.Step == null ? null : JsonConvert.DeserializeObject<List<string[]>>(g.Step),
                    Winner = g.Winner
                })
                .ToArray();

            return result;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Game>> Create(CreateGameData data, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _gameRepository.CreateAsync(data.XPlayerId, data.OPlayerId, cancellationToken);

            return Ok(id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(string id, UpdateGameData data, CancellationToken cancellationToken)
        {
            await _gameRepository.UpdateAsync(int.Parse(id), data.Step, data.Winner, cancellationToken);

            return Ok();
        }
    }
}