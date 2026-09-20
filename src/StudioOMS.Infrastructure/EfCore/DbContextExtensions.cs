using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StudioOMS.EfCore;


public static class DbContextExtensions
{
    extension<TDbContext>(TDbContext dbContext) where TDbContext : DbContext
    {
        public async ValueTask SaveAsync<TEntity, TKey>(Func<TDbContext, DbSet<TEntity>> dbSetSelector, TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : Entity<TKey>
            where TKey : notnull, IEquatable<TKey>
        {
            var entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var exists = await dbSetSelector(dbContext)
                    .AsNoTracking()
                    .AnyAsync(x => x.Id.Equals(entity.Id), cancellationToken);

                if (exists)
                    dbContext.Attach(entity).State = EntityState.Modified;
                else
                    dbContext.Add(entity);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }


    extension<TEntity, TKey>(DbSet<TEntity> dbSet)
        where TEntity : Entity<TKey>
        where TKey : notnull, IEquatable<TKey>
    {
        public async ValueTask<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await dbSet
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
        }

        public async ValueTask<IReadOnlyList<TEntity>?> GetAllAsync<TSelectKey>(Expression<Func<TEntity, TSelectKey>> orderKeySelector, int skip, int take, CancellationToken cancellationToken = default)
        {
            return await dbSet
                .OrderBy(orderKeySelector)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(cancellationToken: cancellationToken);
        }
    }
}