namespace StudioOMS.Orders;


public sealed class TimingOrderConsumeDto
{
    public required decimal Days { get; init; }
    public required string EmployeeId { get; init; }
}