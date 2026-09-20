using Microsoft.Extensions.Caching.Memory;
using StudioOMS.Employees;
using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Security;


internal sealed class SessionService(IMemoryCache memoryCache) : ISessionService
{
    private static readonly TimeSpan SessionLifetime = TimeSpan.FromDays(7);


    public ValueTask<SessionToken> CreateAsync(UserId userId, EmployeeId employeeId, CancellationToken cancellationToken = default)
    {
        var token = SessionToken.Create();
        var info = SessionInfo.Create(userId, employeeId);

        memoryCache.Set(token, info, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = SessionLifetime
        });

        return ValueTask.FromResult(token);
    }

    public ValueTask<SessionInfo?> FindAsync(SessionToken token, CancellationToken cancellationToken = default)
    {
        memoryCache.TryGetValue(token, out SessionInfo? info);
        return ValueTask.FromResult(info);
    }

    public ValueTask RemoveAsync(SessionToken token, CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(token);
        return ValueTask.CompletedTask;
    }
}
