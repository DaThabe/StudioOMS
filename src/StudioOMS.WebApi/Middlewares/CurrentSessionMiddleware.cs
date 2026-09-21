using StudioOMS.Security.Session;

namespace StudioOMS.Middlewares;


internal sealed class CurrentSessionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ISessionService sessions,
        ICurrentSession currentUser)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var tokenStr = authHeader["Bearer ".Length..].Trim();
            if (SessionToken.TryParse(tokenStr, out var token))
            {
                currentUser.Token = token;
                currentUser.Info = await sessions.FindAsync(token);
            }
        }

        await next(context);
    }
}