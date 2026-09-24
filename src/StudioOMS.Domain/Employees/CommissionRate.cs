namespace StudioOMS.Employees;


/// <summary>
/// 提成比率
/// </summary>
public readonly record struct CommissionRate
{
    public static CommissionRate Zero => default;


    public decimal Decimal { get; }
    public decimal Percent => Decimal * 100;

    private CommissionRate(decimal value) => Decimal = value;
    public override string ToString() => $"{Percent:F2}%";



    public static CommissionRate FromDecimal(decimal value)
    {
        if (value < 0 || value > 1)
            throw new ArgumentOutOfRangeException(nameof(value), "提成比率范围 [0~1]");

        return new(value);
    }
    public static CommissionRate FromPercent(decimal value)
    {
        if (value < 0 || value > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "提成比率范围 [0%~100%]");

        return new(value / 100);
    }
}