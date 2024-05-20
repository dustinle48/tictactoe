using Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Domain.Contracts.Repositories;

public interface IGameRepository : IRepository<Game>
{
    Task<int> CreateAsync(int xPlayerId, int oPlayerId, CancellationToken cancellationToken = default);

    Task UpdateAsync(int id, List<List<string>> step, string? winner, CancellationToken cancellationToken = default);
}