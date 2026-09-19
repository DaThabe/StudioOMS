namespace StudioOMS.Orders;


public interface IOrderRepository
{
    ValueTask<Order?> FindByIdAsync(OrderId id, CancellationToken cancellationToken = default);
    ValueTask SaveAsync(Order order, CancellationToken cancellationToken = default);
}