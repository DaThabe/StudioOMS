namespace StudioOMS.Employees;


public readonly record struct EmployeeId
{
    public static EmployeeId Empty => default;

    private readonly Guid _value;
    public EmployeeId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static EmployeeId Create() => new(Guid.CreateVersion7());
    public static EmployeeId Parse(string guid) => new(Guid.Parse(guid));
}