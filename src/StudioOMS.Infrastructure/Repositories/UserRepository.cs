using Microsoft.EntityFrameworkCore;
using StudioOMS.EfCore;
using StudioOMS.Users;

namespace StudioOMS.Repositories;

internal sealed class UserRepository(AppDbContext appDbContext) : Repository<User, UserId>, IUserRepository
{
    protected override AppDbContext DbContext => appDbContext;
    protected override DbSet<User> Entities => appDbContext.Users;


    public async ValueTask<User?> FindByUsername(string username, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        var lowerUsername = username.Trim().ToLower();

        return await appDbContext.Users
            .FirstOrDefaultAsync(x => x.Username.ToLower().Equals(lowerUsername), cancellationToken);
    }

    public ValueTask<IReadOnlyList<User>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default) =>
        GetAllOrderedAsync(x => x.Id, skip, take, cancellationToken);
}
