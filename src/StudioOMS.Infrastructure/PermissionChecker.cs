using StudioOMS.Employees;
using StudioOMS.Permission;

namespace StudioOMS;


internal sealed class PermissionChecker(IEmployeeRepository employeeRepository) : IPermissionChecker
{
    public async ValueTask<bool> HasPermissionAsync(EmployeeId employeeId, PermissionType permission, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.FindByIdAsync(employeeId, cancellationToken);
        if (employee is null) return false;

        return HasPermission(employee.Roles, permission);
    }

    private static bool HasPermission(IReadOnlySet<EmployeeRole> roles, PermissionType permission)
    {
        foreach (var role in roles)
        {
            if (_map.TryGetValue(role, out var permissions) && permissions.Contains(permission))
                return true;
        }

        return false;
    }

    public async ValueTask<bool> HasAllPermissionsAsync(EmployeeId employeeId, IReadOnlySet<PermissionType> permissions, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.FindByIdAsync(employeeId, cancellationToken);
        if (employee is null)
            return false;

        var employeePermissions = GetPermissions(employee.Roles);
        return employeePermissions.IsSupersetOf(permissions);
    }

    public async ValueTask<bool> HasAnyPermissionAsync(EmployeeId employeeId, IReadOnlySet<PermissionType> permissions, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.FindByIdAsync(employeeId, cancellationToken);
        if (employee is null)
            return false;

        var employeePermissions = GetPermissions(employee.Roles);
        return employeePermissions.Overlaps(permissions);
    }


    private static HashSet<PermissionType> GetPermissions(IReadOnlySet<EmployeeRole> roles)
    {
        var result = new HashSet<PermissionType>();

        foreach (var role in roles)
        {
            if (_map.TryGetValue(role, out var permissions))
            {
                foreach (var permission in permissions)
                    result.Add(permission);
            }
        }

        return result;
    }

    private readonly static Dictionary<EmployeeRole, HashSet<PermissionType>> _map = new()
    {
        [EmployeeRole.Admin] = [.. Enum.GetValues<PermissionType>()],

        // 销售
        [EmployeeRole.Sales] =
        [
            PermissionType.OrderRead,
            PermissionType.OrderCreate,
            PermissionType.CustomerRead,
            PermissionType.CustomerCreate
        ],

        // 设计主管
        [EmployeeRole.DesignSupervisor] =
        [
            PermissionType.OrderRead,
            PermissionType.OrderAssign,
            PermissionType.OrderManage,
            PermissionType.EmployeeRead
        ],

        // 设计师
        [EmployeeRole.Designer] =
        [
            PermissionType.OrderRead,
            PermissionType.OrderConsume
        ],

        // 财务
        [EmployeeRole.Finance] =
        [
            PermissionType.OrderRead,
            PermissionType.FinanceRead,
            PermissionType.FinanceSettle
        ]
    };
}
