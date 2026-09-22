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


    void Set(UserId userId, EmployeeId employeeId);
}


public static class CurrentSessionExtensions
{
    extension(ICurrentSession session)
    {
        public bool IsAuthenticated => session.UserId is not null && session.EmployeeId is not null;
    }
}