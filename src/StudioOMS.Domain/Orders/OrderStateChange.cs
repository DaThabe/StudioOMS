namespace StudioOMS.Orders;

/// <summary>
/// 订单状态改变
/// </summary>
public record class OrderStateChange(OrderState State, DateTimeOffset Timestamp) : IComparable<OrderStateChange>
{
    public int CompareTo(OrderStateChange? other) =>
        other is null ? 1 : Timestamp.CompareTo(other.Timestamp);
}
