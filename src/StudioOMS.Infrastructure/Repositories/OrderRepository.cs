using Microsoft.EntityFrameworkCore;
using StudioOMS.EfCore;
using StudioOMS.Orders;

namespace StudioOMS.Repositories;


internal sealed class OrderRepository(AppDbContext appDbContext) : Repository<Order, OrderId>, IOrderRepository
{
    protected override AppDbContext DbContext => appDbContext;
    protected override DbSet<Order> Entities => appDbContext.Orders;


    public ValueTask<IReadOnlyList<Order>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default) =>
        GetAllOrderedAsync(x => x.CreateAt, skip, take, cancellationToken);
}