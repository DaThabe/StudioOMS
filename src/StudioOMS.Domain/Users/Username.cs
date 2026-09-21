namespace StudioOMS.Users;

/// <summary>
/// 一个非空, 前后裁剪, 忽略大小写的字符串
/// </summary>
public sealed record class Username : IEquatable<Username>
{
    private readonly string _value;
    private Username(string value) => _value = value;

    public override string ToString() => _value;
    public bool Equals(Username? other) => string.Equals(_value, other?._value, StringComparison.OrdinalIgnoreCase);
    public override int GetHashCode() => string.GetHashCode(_value, StringComparison.OrdinalIgnoreCase);


    public static Username From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("用户名不可为空", nameof(value));

        var trimmed = value.Trim();
        return new(trimmed);
    }
}
