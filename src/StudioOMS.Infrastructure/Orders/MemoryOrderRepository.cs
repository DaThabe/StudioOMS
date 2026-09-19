namespace StudioOMS.Orders;


internal sealed class MemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<OrderId, Order> _values = [];


    public ValueTask<Order?> FindByIdAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        _values.TryGetValue(id, out var order);
        return ValueTask.FromResult(order);
    }

    public ValueTask SaveAsync(Order order, CancellationToken cancellationToken = default)
    {
        _values[order.Id] = order;
        return ValueTask.CompletedTask;
    }

    public ValueTask<IReadOnlyList<Order>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        var values = _values.OrderBy(x => x.Value.CreateAt)
            .Skip(skip)
            .Take(take)
            .Select(x => x.Value)
            .ToArray();

        return new ValueTask<IReadOnlyList<Order>>(values);
    }
}
