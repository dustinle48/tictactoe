using Domain.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DataAccess.EFCore.Repositories;

public class GameRepository : Repository<Game>, IGameRepository
{
    public GameRepository(TicTacToeDbContext context) : base(context)
    {
    }

    public override IQueryable<Game> GetAll()
    {
        return _context.Games.AsQueryable().AsNoTracking();
    }

    public async Task<int> CreateAsync(int xPlayerId, int oPlayerId, CancellationToken cancellationToken = default)
    {
        if (xPlayerId == oPlayerId)
        {
            throw new Exception("Both players cannot be the same");
        }

        if (oPlayerId == 0 || xPlayerId == 0)
        {
            throw new Exception("Player Id cannot be 0");
        }

        var players = _context.Users.Where(u => u.Id == xPlayerId || u.Id == oPlayerId)
            .ToList();

        var entity = await _context.Games.AddAsync(new Game { XPlayerId = xPlayerId, OPlayerId = oPlayerId, Users = players },
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }

    public async Task UpdateAsync(int id, List<List<string>> step, string? winner,
        CancellationToken cancellationToken = default)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (game == null)
        {
            throw new Exception("Game not found");
        }

        var stepAsString = JsonConvert.SerializeObject(step);
        game.Step = stepAsString;
        game.Winner = winner;

        await _context.SaveChangesAsync(cancellationToken);
    }
}