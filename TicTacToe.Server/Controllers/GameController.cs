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
        private readonly ILogger<GameController> _logger;

        public GameController(IGameRepository gameRepository, ILogger<GameController> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        // TODO: Add [Authorize] that verify both tokenX and tokenY
        [HttpGet]
        public async Task<ActionResult<ResponseGameDto[]>> GetPlayingGames(int xPlayerId, int oPlayerId,
            CancellationToken cancellationToken)
        {
            var httpContext = Request.Headers.Cookie.ToString();
            _logger.LogInformation(httpContext);


            var games = await _gameRepository.GetAll()
                .Where(g => g.XPlayerId == xPlayerId)
                .Where(g => g.OPlayerId == oPlayerId)
                .Where(g => g.Winner == null)
                .ToArrayAsync(cancellationToken);

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

        // TODO: Add [Authorize] that verify both tokenX and tokenY
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