using Newtonsoft.Json;

namespace Domain.Dtos;

public class GameDto
{
    public int Id { get; set; }

    public int XPlayerId { get; set; }

    public int OPlayerId { get; set; }

    public string Step { get; set; }

    public string Winner { get; set; }
}

public class ResponseGameDto
{
    public int Id { get; set; }

    public int XPlayerId { get; set; }

    public int OPlayerId { get; set; }

    public List<string[]>? Step { get; set; }

    public string Winner { get; set; }
}