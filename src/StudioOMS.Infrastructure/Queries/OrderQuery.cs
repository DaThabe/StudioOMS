using Microsoft.EntityFrameworkCore;
using StudioOMS.EfCore;
using StudioOMS.Orders;
using StudioOMS.Requests.Orders;

namespace StudioOMS.Queries;


internal sealed class OrderQuery(AppDbContext appDbContext) : IOrderQuery
{
    public async ValueTask<OrderListResult> QueryAsync(OrderListRequest requesst, CancellationToken cancellationToken = default)
    {
        var orderQuery = appDbContext.Orders.AsNoTracking();
        var types = requesst.Types;

        // 筛选
        if (types.Count != 0)
        {
            orderQuery = orderQuery.Where(order => types.Contains(EF.Property<OrderType>(order, "Type")));
        }

        var orderListItemQuery =
            from order in orderQuery
            join client in appDbContext.Clients on order.ClientId equals client.Id
            select new OrderListItem
            {
                Id = order.Id.ToString(),
                Title = order.Title,
                State = order.State.ToString(),
                CreateAt = order.CreateAt,

                ClientId = order.ClientId.ToString(),
                ClientName = client.Name,
            };

        var items = await orderListItemQuery.ToArrayAsync(cancellationToken: cancellationToken);
        return new OrderListResult() { Items = items };
    }
}