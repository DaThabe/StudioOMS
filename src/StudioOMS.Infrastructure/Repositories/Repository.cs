using Microsoft.EntityFrameworkCore;
using StudioOMS.EfCore;
using System.Linq.Expressions;

namespace StudioOMS.Repositories;

internal abstract class Repository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    protected abstract AppDbContext DbContext { get; }
    protected abstract DbSet<TEntity> Entities { get; }


    public async ValueTask<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await Entities
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
    }

    public async ValueTask SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = DbContext.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            var exists = await Entities
                .AsNoTracking()
                .AnyAsync(x => x.Id.Equals(entity.Id), cancellationToken);

            if (exists)
                DbContext.Attach(entity).State = EntityState.Modified;
            else
                DbContext.Add(entity);
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    protected async ValueTask<IReadOnlyList<TEntity>> GetAllOrderedAsync<TSelectKey>(
        Expression<Func<TEntity, TSelectKey>> orderKeySelector,
        int skip,
        int take,
        CancellationToken ct = default)
    {
        return await Entities
            .OrderBy(orderKeySelector)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(ct);
    }
}