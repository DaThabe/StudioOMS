namespace StudioOMS.Orders;

/// <summary>
/// 订单划扣超过上限
/// </summary>
public abstract class OrderConsumeExceedsLimitException(
    string message,
    OrderId orderId,
    OrderConsumeId orderConsumeId
) : OrderConsumeException(message, orderId, orderConsumeId);
