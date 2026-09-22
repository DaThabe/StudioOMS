namespace StudioOMS.Orders.Timing;

/// <summary>
/// 时间订单划扣超过上限
/// </summary>
public sealed class TimingOrderConsumeExceedsLimitException : OrderConsumeExceedsLimitException
{
    public decimal ExpectDays { get; }
    public decimal ActualDays { get; }

    internal TimingOrderConsumeExceedsLimitException(
            OrderId orderId,
            OrderConsumeId orderConsumeId,
            decimal expectDays,
            decimal actualDays
        ) : base(FormatMessage(orderId, orderConsumeId, expectDays, actualDays), orderId, orderConsumeId)
    {
        ExpectDays = expectDays;
        ActualDays = actualDays;
    }


    private static string FormatMessage(
        OrderId orderId,
        OrderConsumeId orderConsumeId,
        decimal expectDays,
        decimal actualDays)
    {
        return $"订单 {orderId} 划扣 {orderConsumeId} 超过上限 [{expectDays:F2}/{actualDays:F2}] 天";
    }
}