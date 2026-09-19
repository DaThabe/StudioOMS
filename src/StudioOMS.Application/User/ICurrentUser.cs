namespace StudioOMS.User;


/// <summary>
/// 当前用户
/// </summary>
public interface ICurrentUser
{
    EmployeeId EmployeeId { get; }
}