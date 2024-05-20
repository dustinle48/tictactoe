using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Repositories;

    public interface IRepository<TEntity> where TEntity : class, IEntity
{
    IQueryable<TEntity> GetAll();

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
}