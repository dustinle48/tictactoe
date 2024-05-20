using Domain.Contracts;
using Domain.Contracts.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain.Entities;

public class Game : IEntity<int>, IGame
{
    public int Id { get; set; }

    public int XPlayerId { get; set; }

    public int OPlayerId { get; set; }

    [JsonProperty("step")]
    public string? Step { get; set; }

    public string? Winner { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}