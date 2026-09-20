namespace StudioOMS.Security.Permission;


public interface IRequirePermissions
{
    IReadOnlySet<PermissionType> RequiredPermissions { get; }
}