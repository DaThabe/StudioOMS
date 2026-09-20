using StudioOMS.Security.Session;

namespace StudioOMS.Middlewares;


internal sealed class CurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ISessionService sessions,
        ICurrentUser currentUser)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var tokenStr = authHeader["Bearer ".Length..].Trim();
            if (SessionToken.TryParse(tokenStr, out var token))
            {
                var session = await sessions.FindAsync(token);
                if (session is not null) currentUser.EmployeeId = session.EmployeeId;
            }
        }

        await next(context);
    }
}