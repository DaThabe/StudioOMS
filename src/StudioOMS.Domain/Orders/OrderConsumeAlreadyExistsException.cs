namespace StudioOMS.Orders;

/// <summary>
/// 订单划扣重复异常
/// </summary>
public sealed class OrderConsumeAlreadyExistsException : OrderConsumeException
{
    internal OrderConsumeAlreadyExistsException(
        OrderId orderId,
        OrderConsumeId orderConsumeId
    ) : base(FormatMessage(orderId, orderConsumeId), orderId, orderConsumeId) { }

    private static string FormatMessage(OrderId orderId, OrderConsumeId orderConsumeId)
    {
        return $"订单 {orderId} 划扣 {orderConsumeId} 已存在";
    }
}
