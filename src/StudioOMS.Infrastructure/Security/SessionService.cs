using Microsoft.Extensions.Caching.Memory;
using StudioOMS.Employees;
using StudioOMS.Security.Session;
using StudioOMS.Users;
using System.Collections.Concurrent;

namespace StudioOMS.Security;


internal sealed class SessionService(IMemoryCache memoryCache) : ISessionService
{
    private static readonly TimeSpan SessionLifetime = TimeSpan.FromDays(7);
    private readonly ConcurrentDictionary<UserId, SessionToken> _userTokens = new();


    public ValueTask<SessionToken> CreateAsync(UserId userId, EmployeeId employeeId, CancellationToken cancellationToken = default)
    {
        // 删除该用户的旧会话
        if (_userTokens.TryRemove(userId, out var oldToken))
            memoryCache.Remove(oldToken);

        var token = SessionToken.Create();
        var info = SessionInfo.Create(userId, employeeId);

        memoryCache.Set(token, info, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = SessionLifetime
        });

        _userTokens[userId] = token;
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

        var entry = _userTokens.FirstOrDefault(kvp => kvp.Value == token);
        if (entry.Key != default)
            _userTokens.TryRemove(entry.Key, out _);

        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        if (_userTokens.TryGetValue(userId, out var token))
        {
            memoryCache.Remove(userId);
            _userTokens.Remove(userId, out _);
        }

        return ValueTask.CompletedTask;
    }
}
