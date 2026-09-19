namespace StudioOMS.Orders.Timing;


public sealed partial record class TimingOrder : Order
{
    public required decimal TotalDays { get; init; }
    public decimal UsedDays { get; private set; }
}