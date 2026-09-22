using StudioOMS.Exceptions;

namespace StudioOMS.Orders;

/// <summary>
/// 订单划扣异常基类
/// </summary>
public abstract class OrderConsumeException(
    string message,
    OrderId orderId,
    OrderConsumeId orderConsumeId
) : StudioOMSException(message)
{
    public OrderId OrderId { get; } = orderId;
    public OrderConsumeId OrderConsumeId { get; } = orderConsumeId;
}
