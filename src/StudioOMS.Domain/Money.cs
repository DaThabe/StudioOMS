namespace StudioOMS;


public readonly record struct Money : IEquatable<Money>, IComparable<Money>
{
    public decimal Amount { get; }
    public string Currency { get => field ?? string.Empty; }


    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// 人民币
    /// </summary>
    public static Money CNY(decimal amount) =>
        new(amount, "CNY");

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" />
    public static Money From(decimal amount, string currency)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        return currency.Trim().ToUpperInvariant() switch
        {
            "CNY" => CNY(amount),
            _ => throw new ArgumentException($"不支持的货币类型: {currency}", nameof(currency))
        };
    }



    public int CompareTo(Money other)
    {
        // 金额比较
        var cmp = Amount.CompareTo(other.Amount);
        if (cmp != 0) return cmp;
        // 货币比较
        return string.Compare(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);
    }
    public bool Equals(Money money)
    {
        if (money.Amount != Amount) return false;
        return string.Equals(Currency, money.Currency, StringComparison.OrdinalIgnoreCase);
    }
    public override int GetHashCode()
    {
        var code = new HashCode();
        code.Add(Amount);
        code.Add(Currency.GetHashCode(StringComparison.OrdinalIgnoreCase));
        return code.ToHashCode();
    }
    public override string ToString()
    {
        return $"{Amount:N2} {Currency}";
    }
}