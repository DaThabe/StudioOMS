using StudioOMS.Employees;

namespace StudioOMS.Orders;


public abstract class OrderConsume : Entity<OrderConsumeId>
{
    public required EmployeeId EmployeeId { get; init; }
    public required DateTimeOffset Timestamp { get; init; }


    protected OrderConsume() { }
}