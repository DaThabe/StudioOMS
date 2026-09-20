namespace StudioOMS.Orders;


public record class OrderAssignEmployeeDto
{
    public required Guid EmployeeId { get; init; }
}
