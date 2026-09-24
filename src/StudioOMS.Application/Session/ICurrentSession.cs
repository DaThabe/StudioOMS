using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.Session;


/// <summary>
/// 当前会话
/// </summary>
public interface ICurrentSession
{
    UserId UserId { get; }
    EmployeeId EmployeeId { get; }
    bool IsAuthenticated { get; }


    void Set(UserId userId, EmployeeId employeeId);
}