using StudioOMS.Employees;

namespace StudioOMS.Orders;


public abstract class OrderConsume : Entity<ConsumeId>
{
    public required EmployeeId EmployeeId { get; init; }
    public required DateTimeOffset Timestamp { get; init; }


    protected OrderConsume() { }
}


public readonly record struct ConsumeId
{
    public static ConsumeId Empty => default;

    private readonly Guid _value;
    public ConsumeId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static ConsumeId Create() => new(Guid.CreateVersion7());
    public static ConsumeId Parse(string guid) => new(Guid.Parse(guid));
}