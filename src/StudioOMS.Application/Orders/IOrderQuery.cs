namespace StudioOMS.Orders;

public interface IOrderQuery
{
    ValueTask<OrderListResponse> QueryAsync(OrderListRequest requesst, CancellationToken cancellationToken = default);
}
