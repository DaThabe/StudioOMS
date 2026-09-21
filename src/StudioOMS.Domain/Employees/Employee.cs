namespace StudioOMS.Employees;


public sealed class Employee : Entity<EmployeeId>
{
    private HashSet<EmployeeRole> _roles = [];

    public EmployeeName Name { get; private set; }
    public IReadOnlySet<EmployeeRole> Roles => _roles.AsReadOnly();



    public void Rename(EmployeeName value)
    {
        if (value.IsExactlySameAs(Name)) return;

        // 更新
        Name = value;
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



    private Employee(EmployeeName name) => Name = name;
    public static Employee Create(EmployeeId employeeId, EmployeeName name, IEnumerable<EmployeeRole> roles)
    {
        var roleSet = roles.ToHashSet();
        if (roleSet.Count == 0)
            throw new ArgumentException("员工 职位 不可为空", nameof(roles));


        return new(name)
        {
            _roles = [.. roleSet],
            Id = employeeId
        };
    }
    public static Employee Create(EmployeeName name, IEnumerable<EmployeeRole> roles) =>
        Create(EmployeeId.Create(), name, roles);
}