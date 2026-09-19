namespace StudioOMS;


public readonly record struct EmployeeId
{
    public static EmployeeId Empty => default;

    private readonly Guid _value;
    private EmployeeId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static EmployeeId Create() => new(Guid.CreateVersion7());
}