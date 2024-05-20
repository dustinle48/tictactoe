using Newtonsoft.Json.Linq;

namespace TicTacToe.Server.Models;

public class CreateGameData
{
    public int XPlayerId { get; set; }

    public int OPlayerId { get; set; }
}

public class UpdateGameData
{
    public int Id { get; set; }

    public int XPlayerId { get; set; }

    public int OPlayerId { get; set; }

    public List<List<string>> Step { get; set; }

    public string? Winner { get; set; }
}