namespace StudioOMS.Customers;


/// <summary>
/// 长度<see cref="MinLength"/>~<see cref="MaxLength"/> <br/>
/// 不包含控制字符 <br/>
/// 大小写不敏感
/// </summary>
public sealed record class CustomerName : IEquatable<CustomerName>
{
    public const int MinLength = 1;
    public const int MaxLength = 100;


    private readonly string _value;
    private CustomerName(string value) => _value = value;
    public override string ToString() => _value;

    public bool Equals(CustomerName? other) =>
        other is not null && string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
    public override int GetHashCode() =>
        StringComparer.OrdinalIgnoreCase.GetHashCode(_value);

    /// <summary>
    /// 是否完全一样 (大小写也一样) <br/>
    /// ABC 和 abc 不相同
    /// </summary>
    public bool IsExactlySameAs(CustomerName? other) =>
        other is not null && string.Equals(_value, other._value, StringComparison.Ordinal);


    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    public static CustomerName From(ReadOnlySpan<char> value)
    {
        var trimmed = value.Trim();

        if (trimmed.Length < MinLength || trimmed.Length > MaxLength)
            throw new ArgumentException($"客户名长度需要在 [{MinLength}~{MaxLength}] 之间", nameof(value));

        foreach (var c in trimmed)
        {
            if (char.IsControl(c))
                throw new ArgumentException("客户名不可含特殊字符", nameof(value));
        }

        return new(trimmed.ToString());
    }
}