using StudioOMS.Exceptions;

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
            throw new EmployeeMustHaveRoleException(Id);

        _roles.Clear();
        foreach (var role in remaining) _roles.Add(role);
    }



    private Employee(EmployeeName name) => Name = name;
    public static Employee Create(EmployeeId employeeId, EmployeeName name, IEnumerable<EmployeeRole> roles)
    {
        var roleSet = roles.ToHashSet();
        if (roleSet.Count == 0)
            throw new EmployeeMustHaveRoleException(employeeId);

        return new(name)
        {
            _roles = [.. roleSet],
            Id = employeeId
        };
    }
    public static Employee Create(EmployeeName name, IEnumerable<EmployeeRole> roles) =>
        Create(EmployeeId.Create(), name, roles);
}


/// <summary>
/// 员工至少有一个职位
/// </summary>
public sealed class EmployeeMustHaveRoleException : StudioOMSException
{
    public EmployeeId EmployeeId { get; }

    internal EmployeeMustHaveRoleException(EmployeeId employeeId) : base($"员工 {employeeId} 至少需要保留一个职位")
    {
        EmployeeId = employeeId;
    }
}