using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.Security.Session;

public sealed record class SessionInfo
{
    public required UserId UserId { get; init; } 
    public required EmployeeId EmployeeId { get; init; }


    private SessionInfo() { }
    public static SessionInfo Create(UserId userId, EmployeeId employeeId)
    {
        if (userId == UserId.Empty)
            throw new ArgumentException("用户 Id 不可为空", nameof(userId));

        if (employeeId == EmployeeId.Empty)
            throw new ArgumentException("员工 Id 不可为空", nameof(employeeId));

        return new()
        {
            UserId = userId,
            EmployeeId = employeeId
        };
    }
}