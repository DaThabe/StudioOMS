using Microsoft.EntityFrameworkCore;
using StudioOMS.Customers;
using StudioOMS.EfCore;

namespace StudioOMS.Repositories;


internal sealed class CustomerRepository(AppDbContext appDbContext) : Repository<Customer, CustomertId>, ICustomerRepository
{
    protected override AppDbContext DbContext => appDbContext;
    protected override DbSet<Customer> Entities => appDbContext.Customers;

    public ValueTask<IReadOnlyList<Customer>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default) =>
        GetAllOrderedAsync(x => x.Id, skip, take, cancellationToken);
}
