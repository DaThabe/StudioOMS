using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.Security.Session;


public interface ISessionService
{
    ValueTask<SessionToken> CreateAsync(UserId userId, EmployeeId employeeId, CancellationToken cancellationToken = default);
    ValueTask<SessionInfo?> FindAsync(SessionToken token, CancellationToken cancellationToken = default);
    ValueTask RemoveAsync(SessionToken token, CancellationToken cancellationToken = default);
}