using StudioOMS.Customers;
using StudioOMS.Employees;

namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial class Order : Entity<OrderId>
{
    /// <summary>
    /// 客户Id
    /// </summary>
    public required CustomertId CustomerId { get; init; }
    /// <summary>
    /// 销售员Id
    /// </summary>
    public required EmployeeId SalespersonId { get; init; }
}


public readonly record struct OrderId : IEquatable<OrderId>
{
    public static OrderId Empty => default;

    private readonly Guid _value;
    public OrderId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static OrderId Create() => new(Guid.CreateVersion7());
    public static OrderId Parse(string guid) => new(Guid.Parse(guid));
}