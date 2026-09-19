namespace StudioOMS.Orders;


public interface IOrderRepository
{
    ValueTask SaveAsync(Order order, CancellationToken cancellationToken = default);
    ValueTask<Order?> FindByIdAsync(OrderId id, CancellationToken cancellationToken = default);
    ValueTask<IReadOnlyList<Order>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);
}