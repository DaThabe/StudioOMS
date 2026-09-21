using Microsoft.EntityFrameworkCore;
using StudioOMS.EfCore;
using StudioOMS.Orders;

namespace StudioOMS.Queries;


internal sealed class OrderQuery(AppDbContext appDbContext) : IOrderQuery
{
    public async ValueTask<OrderListResponse> QueryAsync(OrderListRequest requesst, CancellationToken cancellationToken = default)
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
            join customer in appDbContext.Customers on order.CustomerId equals customer.Id
            select new OrderListItem
            {
                Id = order.Id,
                Title = order.Title,
                State = order.State,
                CreateAt = order.CreateAt,
                Type = EF.Property<OrderType>(order, "Type"),

                CustomerId = order.CustomerId,
                CustomerName = customer.Name,
            };

        var items = await orderListItemQuery.ToArrayAsync(cancellationToken: cancellationToken);
        return new OrderListResponse() { Items = items };
    }
}