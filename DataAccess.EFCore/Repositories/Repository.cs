using Domain.Contracts.Repositories;

namespace DataAccess.EFCore.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly TicTacToeDbContext _context;
    public Repository(TicTacToeDbContext context)
    {
        _context = context;
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));

        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);

        return entity;
    }
}