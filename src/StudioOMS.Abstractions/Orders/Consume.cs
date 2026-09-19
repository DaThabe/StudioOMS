namespace StudioOMS.Orders;


public abstract record class Consume : IEquatable<Consume>
{
    public required ConsumeId Id { get; init; }
    public required EmployeeId EmployeeId { get; init; }
    public required DateTime Timestamp { get; init; }


    protected Consume() { }

    public virtual bool Equals(Consume? ohter) => Id.Equals(ohter?.Id);
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Id.ToString() ?? string.Empty;
}


public readonly record struct ConsumeId
{
    public static ConsumeId Empty => default;

    private readonly Guid _value;
    private ConsumeId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static ConsumeId Create() => new(Guid.CreateVersion7());
}