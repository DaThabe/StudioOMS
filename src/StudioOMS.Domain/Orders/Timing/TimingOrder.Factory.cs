using StudioOMS.Customers;
using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed partial class TimingOrder : Order
{
    private TimingOrder() { }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static TimingOrder Create(OrderId orderId, CustomertId customerId, EmployeeId salespersonId, Money price, decimal totalDays, DateTimeOffset createTime)
    {
        ArgumentNullException.ThrowIfNull(orderId);
        ArgumentNullException.ThrowIfNull(customerId);
        ArgumentNullException.ThrowIfNull(salespersonId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price.Amount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalDays);

        return new()
        {
            Id = orderId,
            CustomerId = customerId,
            SalespersonId = salespersonId,
            Price = price,
            TotalDays = totalDays,
            CreateAt = createTime
        };
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static TimingOrder Create(CustomertId customerId, EmployeeId salespersonId, Money price, decimal totalDays, DateTimeOffset createTime) =>
        Create(OrderId.Create(), customerId, salespersonId, price, totalDays, createTime);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static TimingOrder CreateNow(CustomertId customerId, EmployeeId salespersonId, Money price, decimal totalDays) =>
        Create(customerId, salespersonId, price, totalDays, DateTimeOffset.UtcNow);
}