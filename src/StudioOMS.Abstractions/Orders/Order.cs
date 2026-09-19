namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial record class Order : IEquatable<Order>
{
    /// <summary>
    /// 订单Id
    /// </summary>
    public required OrderId Id { get; init; }
    /// <summary>
    /// 客户Id
    /// </summary>
    public required ClientId ClientId { get; init; }
    /// <summary>
    /// 销售员Id
    /// </summary>
    public required EmployeeId SalespersonId { get; init; }


    public virtual bool Equals(Order? other) => Id.Equals(other?.Id);
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Id.ToString() ?? string.Empty;
}


public readonly record struct OrderId
{
    public static OrderId Empty => default;

    private readonly Guid _value;
    private OrderId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static OrderId Create() => new(Guid.CreateVersion7());
}