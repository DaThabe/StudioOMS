namespace StudioOMS.Employees;


public sealed class Employee : Entity<EmployeeId>
{
    private readonly HashSet<EmployeeRole> _roles = [];

    public string Name { get; private set; } = "未命名员工";
    public IReadOnlySet<EmployeeRole> Roles => _roles.AsReadOnly();



    public void Rename(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var trimmed = value.Trim();

        if (string.Equals(trimmed, Name, StringComparison.OrdinalIgnoreCase))
            return;

        Name = trimmed;
    }

    public void AddRoles(params IEnumerable<EmployeeRole> roles)
    {
        _roles.UnionWith(roles);
    }
    public void RemoveRoles(params IEnumerable<EmployeeRole> roles)
    {
        foreach (var i in roles.ToArray()) _roles.Remove(i);
    }



    internal Employee() { }
    public static Employee Create(EmployeeId employeeId)
    {
        if (employeeId == EmployeeId.Empty)
            throw new ArgumentException("员工 Id 不可为空", nameof(employeeId));


        return new() { Id = employeeId };
    }
    public static Employee Create() =>
        Create(EmployeeId.Create());
}