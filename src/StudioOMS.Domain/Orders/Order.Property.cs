namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial record class Order
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; private set; } = "未命名订单";


    /// <summary>
    /// 更改标题
    /// </summary>
    public PropertyChangedResult ChangeTitle(string title)
    {
        // 空字符串
        if (string.IsNullOrWhiteSpace(title))
            return PropertyChangedResult.InvalidValue("标题不可为空");
        // 没变化
        var trimmed = title.Trim();
        if (string.Equals(Title, trimmed))
            return PropertyChangedResult.Success;


        Title = trimmed;
        return PropertyChangedResult.Success;
    }
}


public abstract record class PropertyChangedResult
{
    public static SuccessResult Success { get; } = new();
    public static InvalidValueResult InvalidValue(string reason) => new()
    {
        Reason = reason
    };


    internal PropertyChangedResult() { }
    public sealed record class SuccessResult : PropertyChangedResult
    {
        internal SuccessResult() { }
        public override string ToString() => "属性更改成功";
    }
    public sealed record class InvalidValueResult : PropertyChangedResult
    {
        public required string Reason { get; init; }
        internal InvalidValueResult() { }
        public override string ToString() => $"属性更改失败: {Reason}";
    }
}