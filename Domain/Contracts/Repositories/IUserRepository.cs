using Domain.Dtos;
using Domain.Entities;

namespace Domain.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<UserDto?> GetByNameAndPasswordAsync(string name, string password, 
        CancellationToken cancellationToken = default);

    Task CreateAsync(string name, string password, CancellationToken cancellationToken = default);
}