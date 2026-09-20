namespace StudioOMS.Orders.Timing;


public sealed class TimingOrderConsumeDto
{
    public required decimal Days { get; init; }
    public required Guid EmployeeId { get; init; }
}