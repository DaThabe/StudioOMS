namespace StudioOMS;


public abstract record class PropertyChangedResult
{
    public static SuccessResult Success { get; } = new();
    public static NotChangeResult NotChange { get; } = new();
    public static InvalidValueResult InvalidValue(string reason) => new()
    {
        Reason = reason
    };


    internal PropertyChangedResult() { }

    public sealed record class NotChangeResult : PropertyChangedResult
    {
        internal NotChangeResult() { }
        public override string ToString() => $"属性未变更";
    }
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