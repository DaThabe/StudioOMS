using StudioOMS.Security.Permission;

namespace StudioOMS.Messaging;


/// <summary>
/// 访问约束
/// </summary>
public interface IAccessConstraint;


/// <summary>
/// 无需认证
/// </summary>
public interface IAllowAnonymous : IAccessConstraint;


/// <summary>
/// 需要认证 (需要用户登录)
/// </summary>
public interface IAuthentication : IAccessConstraint;

/// <summary>
/// 需要授权 (需要用户登录且拥有权限)
/// </summary>
public interface IAuthorization : IAuthentication
{
    IReadOnlySet<PermissionType> RequiredPermissions { get; }
}