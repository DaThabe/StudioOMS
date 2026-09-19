namespace StudioOMS.Orders.Timing;


public abstract record class ConsumeResult
{
    public static SuccessResult Success { get; } = new();
    public static NotServicingResult NotServicing { get; } = new();
    public static RepeatedResult Repeated { get; } = new();
    public static ExceedResult Exceed(decimal currentDays, decimal totalDays) => new()
    {
        CurrentDays = currentDays,
        TotalDays = totalDays
    };


    /// <summary>
    /// 成功
    /// </summary>
    public sealed record class SuccessResult : ConsumeResult
    {
        internal SuccessResult() { }
    }

    /// <summary>
    /// 订单未服务
    /// </summary>
    public sealed record class NotServicingResult : ConsumeResult
    {
        internal NotServicingResult() { }
    }

    /// <summary>
    /// 重复消耗
    /// </summary>
    public sealed record class RepeatedResult : ConsumeResult
    {
        internal RepeatedResult() { }
    }

    /// <summary>
    /// 超过期限
    /// </summary>
    public sealed record class ExceedResult : ConsumeResult
    {
        public required decimal CurrentDays { get; init; }
        public required decimal TotalDays { get; init; }

        internal ExceedResult() { }
    }
}