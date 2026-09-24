using StudioOMS.Employees;
using StudioOMS.Permission;
using StudioOMS.Users;

namespace StudioOMS.Exceptions;


/// <summary>
/// 权限不足异常
/// </summary>
#pragma warning disable RCS1194 // Implement exception constructors
public sealed class ForbiddenException(
#pragma warning restore RCS1194 // Implement exception constructors
    UserId userId,
    EmployeeId employeeId,
    IReadOnlySet<PermissionType> allowPermissions
) : AppException("权限不足")
{
    public UserId UserId { get; } = userId;
    public EmployeeId EmployeeId { get; } = employeeId;
    public IReadOnlySet<PermissionType> Permissions { get; } = allowPermissions;
}
#pragma warning restore RCS1194 // Implement exception constructors