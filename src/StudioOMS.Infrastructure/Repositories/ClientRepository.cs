using Microsoft.EntityFrameworkCore;
using StudioOMS.Clients;
using StudioOMS.EfCore;

namespace StudioOMS.Repositories;

internal sealed class ClientRepository(AppDbContext appDbContext) : Repository<Client, ClientId>, IClientRepository
{
    protected override AppDbContext DbContext => appDbContext;
    protected override DbSet<Client> Entities => appDbContext.Clients;

    public ValueTask<IReadOnlyList<Client>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default) =>
        GetAllOrderedAsync(x => x.Id, skip, take, cancellationToken);
}
