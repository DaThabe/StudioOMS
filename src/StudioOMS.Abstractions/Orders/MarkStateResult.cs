namespace StudioOMS.Orders;


/// <summary>
/// 订单状态标记结果
/// </summary>
public abstract record class MarkStateResult
{
    internal static SuccessResult Success(OrderState state) => new()
    {
        Marked = state
    };

    internal static NotAsExpectedResult NotAsExpected(OrderState actual, params IEnumerable<OrderState> expect) => new()
    {
        Actual = actual,
        Expects = expect.ToHashSet()
    };


    /// <summary>
    /// 成功
    /// </summary>
    public sealed record class SuccessResult : MarkStateResult
    {
        public required OrderState Marked { get; init; }
        internal SuccessResult() { }
    }

    /// <summary>
    /// 不符合预期
    /// </summary>
    public sealed record class NotAsExpectedResult : MarkStateResult
    {
        public required OrderState Actual { get; init; }
        public required IReadOnlySet<OrderState> Expects { get; init; }
        internal NotAsExpectedResult() { }
    }
}