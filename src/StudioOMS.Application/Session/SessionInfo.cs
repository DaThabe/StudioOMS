using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.Session;


public sealed record class SessionInfo
{
    public required UserId UserId { get; init; }
    public required EmployeeId EmployeeId { get; init; }
    public required DateTimeOffset ExpiresAt { get; init; }


    /// <summary>
    /// 是否过期
    /// </summary>
    public bool IsExpired(DateTimeOffset now) => ExpiresAt <= now;
}