namespace Domain.Dtos;

public class UserDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public List<GameDto> Games { get; set; } = new List<GameDto>();

    public string? Token { get; set; }
}