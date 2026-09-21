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
        return new()
        {
            UserId = userId,
            EmployeeId = employeeId
        };
    }
}