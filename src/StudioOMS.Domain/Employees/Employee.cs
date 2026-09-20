namespace StudioOMS.Employees;


public sealed class Employee : Entity<EmployeeId>
{
    private HashSet<EmployeeRole> _roles = [];

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
        var remaining = _roles.Except(roles).ToHashSet();

        if (remaining.Count == 0)
            throw new InvalidOperationException("员工至少保留一个角色");

        _roles.Clear();
        foreach (var role in remaining) _roles.Add(role);
    }



    internal Employee() { }
    public static Employee Create(EmployeeId employeeId, IEnumerable<EmployeeRole> roles)
    {
        if (employeeId == EmployeeId.Empty)
            throw new ArgumentException("员工 Id 不可为空", nameof(employeeId));

        var roleSet = roles.ToHashSet();
        if (roleSet.Count == 0)
            throw new ArgumentException("员工 职位 不可为空", nameof(roles));


        return new()
        {
            _roles = [.. roleSet],
            Id = employeeId
        };
    }
    public static Employee Create(IEnumerable<EmployeeRole> roles) =>
        Create(EmployeeId.Create(), roles);
}