using StudioOMS.Session;

namespace StudioOMS.Middlewares;


internal sealed class CurrentSessionMiddleware(
    RequestDelegate next,
    TimeProvider timeProvider,
    ISessionInfoRepository sessionInfoRepository
    )
{
    public async Task InvokeAsync(
        HttpContext context,
        ICurrentSession currentUser)
    {
        var token = GetSessionToken(context);
        await SetCurrentUser(token, currentUser, context.RequestAborted);

        await next(context);
    }

    // 设置当前用户
    private async Task<bool> SetCurrentUser(
        SessionToken? token,
        ICurrentSession currentUser,
        CancellationToken cancellationToken)
    {
        if (token is null) return false;

        // 会话不存在
        var info = await sessionInfoRepository.FindAsync(token, cancellationToken);
        if (info is null) return false;

        // 会话过期
        if (info.IsExpired(timeProvider.GetUtcNow())) return false;

        // 成功
        currentUser.Set(info.UserId, info.EmployeeId);
        return true;
    }

    // 获取令牌
    private static SessionToken? GetSessionToken(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var tokenStr = authHeader["Bearer ".Length..].Trim();
        if (!SessionToken.TryParse(tokenStr, out var token))
        {
            return null;
        }

        return token;
    }
}