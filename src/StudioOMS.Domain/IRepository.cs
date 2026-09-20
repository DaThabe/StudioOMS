namespace StudioOMS;


public interface IRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    ValueTask<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);
    ValueTask SaveAsync(TEntity entity, CancellationToken cancellationToken = default);
    ValueTask<IReadOnlyList<TEntity>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);
}