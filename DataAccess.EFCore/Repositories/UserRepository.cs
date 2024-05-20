using Domain.Contracts.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TicTacToeDbContext context) : base(context)
    {
    }

    public override IQueryable<User> GetAll()
    {
        return _context.Users.AsQueryable().AsNoTracking();
    }

    public async Task<UserDto?> GetByNameAndPasswordAsync(string name, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.Where(user => user.Name == name && user.Password == password)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Password = u.Password,
                Games = u.Games.Select(g => new GameDto
                {
                    Id = g.Id,
                    XPlayerId = g.XPlayerId,
                    OPlayerId = g.OPlayerId,
                    Step = g.Step,
                    Winner = g.Winner,
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }

    public async Task CreateAsync(string name, string password, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(new User { Name = name, Password = password }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}