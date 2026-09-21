namespace StudioOMS.Employees;


/// <summary>
/// 长度<see cref="MinLength"/>~<see cref="MaxLength"/> <br/>
/// 不包含控制字符 <br/>
/// 大小写不敏感
/// </summary>
public sealed record class EmployeeName : IEquatable<EmployeeName>
{
    public const int MinLength = 2;
    public const int MaxLength = 30;


    private readonly string _value;
    private EmployeeName(string value) => _value = value;
    public override string ToString() => _value;

    public bool Equals(EmployeeName? other) =>
        other is not null && string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
    public override int GetHashCode() =>
        StringComparer.OrdinalIgnoreCase.GetHashCode(_value);


    /// <summary>
    /// 是否完全一样 (大小写也一样) <br/>
    /// ABC 和 abc 不相同
    /// </summary>
    public bool IsExactlySameAs(EmployeeName? other) =>
        other is not null && string.Equals(_value, other._value, StringComparison.Ordinal);

    public static EmployeeName From(ReadOnlySpan<char> value)
    {
        var trimmed = value.Trim();

        if (trimmed.Length < MinLength || trimmed.Length > MaxLength)
            throw new ArgumentException($"员工名长度需要在 [{MinLength}~{MaxLength}] 之间", nameof(value));

        foreach (var c in value)
        {
            if (char.IsControl(c))
                throw new ArgumentException("员工名不可含特殊字符", nameof(value));
        }

        var name = string.Create(value.Length, value, static (span, source) => source.CopyTo(span));
        return new EmployeeName(name);
    }
}