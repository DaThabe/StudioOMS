using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudioOMS.EfCore;
using StudioOMS.Security;
using StudioOMS.Users;

namespace StudioOMS.Session;


internal sealed class SessionManager(
        TimeProvider timeProvider,
        IOptions<SessionOptions> options,
        AppDbContext appDbContext,
        ISessionInfoRepository sessionInfoRepository
    ) : ISessionManager
{
    public async ValueTask<SessionToken> SignInAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var token = SessionToken.Create();
        var expiresAt = timeProvider.GetUtcNow().Add(options.Value.Lifetime);
        var info = await CreateSessionInfoAsync(userId, expiresAt, cancellationToken);

        await sessionInfoRepository.SaveAsync(token, info, cancellationToken);
        return token;
    }


    public ValueTask SignOutAsync(SessionToken token, CancellationToken cancellationToken = default)
    {
        return sessionInfoRepository.RemoveAsync(token, cancellationToken);
    }
    public ValueTask SignOutAllAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return sessionInfoRepository.RemoveByUserIdAsync(userId, cancellationToken);
    }


    // 创建会话信息
    private async Task<SessionInfo> CreateSessionInfoAsync(UserId userId, DateTimeOffset expiresAt, CancellationToken cancellationToken)
    {
        var query =
            from user in appDbContext.Users.AsNoTracking()
            where user.Id == userId
            select new SessionInfo
            {
                UserId = user.Id,
                EmployeeId = user.EmployeeId,
                ExpiresAt = expiresAt
            };

        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException($"用户 {userId} 不存在");
    }
}
