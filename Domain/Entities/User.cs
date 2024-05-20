using Domain.Contracts;
using Domain.Contracts.Repositories;

namespace Domain.Entities;

public class User : IEntity<int>, IUser
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public ICollection<Game>? Games { get; set; }
}