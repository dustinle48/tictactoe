namespace Domain.Contracts.Repositories;

public interface IEntity<out TKey> : IEntity
{
    TKey Id { get; }
}

public interface IEntity
{
}