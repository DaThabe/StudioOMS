using Microsoft.Extensions.Caching.Memory;
using StudioOMS.Users;
using System.Collections.Concurrent;

namespace StudioOMS.Session;


internal sealed class SessionInfoRepository(
        IMemoryCache memoryCache
    ) : ISessionInfoRepository
{
    /// <summary>
    /// 所有用户的所有令牌
    /// </summary>
    private readonly ConcurrentDictionary<UserId, HashSet<SessionToken>> _users = new();


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
    public ValueTask RemoveByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        if (_users.TryRemove(userId, out var tokens))
        {
            foreach (var token in tokens.ToArray())
                memoryCache.Remove(token);
        }

        return ValueTask.CompletedTask;
    }


    public ValueTask SaveAsync(SessionToken token, SessionInfo info, CancellationToken cancellationToken = default)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = info.ExpiresAt,
            PostEvictionCallbacks =
            {
                new PostEvictionCallbackRegistration
                {
                    EvictionCallback = (key, value, _, __) =>
                    {
                        if(key is not SessionToken token) return;
                        if (value is not SessionInfo info) return;

                        // 只有反向索引仍包含这个 token 时才删除
                        if (_users.TryGetValue(info.UserId, out var tokens) && tokens.Contains(token))
                            tokens.Remove(token);
                    }
                }
            }
        };

        // 缓存token -> 会话信息
        memoryCache.Set(token, info, options);

        // 关联用户
        if (!_users.TryGetValue(info.UserId, out var tokens))
            tokens = _users[info.UserId] = tokens ?? [];
        // 追加令牌
        tokens.Add(token);

        return ValueTask.CompletedTask;
    }
}