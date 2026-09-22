using StudioOMS.Employees;

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
            EmployeeId employeeId,
            decimal expectDays,
            decimal actualDays
        ) : base(FormatMessage(orderId, employeeId, expectDays, actualDays), orderId, employeeId)
    {
        ExpectDays = expectDays;
        ActualDays = actualDays;
    }


    private static string FormatMessage(
        OrderId orderId,
        EmployeeId employeeId,
        decimal expectDays,
        decimal actualDays)
    {
        return $"订单 {orderId} 划扣超过上限 [{expectDays:F2}/{actualDays:F2}] 天";
    }
}