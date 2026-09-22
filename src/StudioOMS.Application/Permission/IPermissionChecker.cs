using StudioOMS.Employees;

namespace StudioOMS.Permission;


public interface IPermissionChecker
{
    /// <summary>
    /// 这个员工是否拥有指定的权限
    /// </summary>
    /// <param name="employeeId">员工Id</param>
    /// <param name="permission">权限类型</param>
    ValueTask<bool> HasPermissionAsync(EmployeeId employeeId, PermissionType permission, CancellationToken cancellationToken = default);

    /// <summary>
    /// 这个员工包含所有权限
    /// </summary>
    /// <param name="employeeId">员工Id</param>
    /// <param name="permissions">权限类型</param>
    ValueTask<bool> HasAllPermissionsAsync(EmployeeId employeeId, IReadOnlySet<PermissionType> permissions, CancellationToken cancellationToken = default);

    /// <summary>
    /// 这个员工包含任意一个权限
    /// </summary>
    /// <param name="employeeId">员工Id</param>
    /// <param name="permissions">权限类型</param>
    ValueTask<bool> HasAnyPermissionAsync(EmployeeId employeeId, IReadOnlySet<PermissionType> permissions, CancellationToken cancellationToken = default);
}