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