using Microsoft.Extensions.DependencyInjection;
using StudioOMS.Requests;
using StudioOMS.Security.Permission;
using StudioOMS.Security.Session;

namespace StudioOMS;


internal sealed class Sender(IServiceProvider services, ICurrentUser currentUser, IPermissionChecker permissionChecker) : ISender
{
    public async ValueTask SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        var handler = services.GetRequiredService<IRequestHandler<TRequest>>();

        await AssertCheckPermission(handler);
        await handler.HandleAsync(request, cancellationToken);
    }

    public async ValueTask<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handler = services.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        await AssertCheckPermission(handler);
        return await handler.HandleAsync(request, cancellationToken);
    }

    private async ValueTask AssertCheckPermission(object handler)
    {
        if (handler is not IRequirePermissions requirePermissions)
            return;

        var pass = await permissionChecker.HasAllPermissionsAsync(currentUser.EmployeeId, requirePermissions.RequiredPermissions);
        if (!pass) throw new UnauthorizedAccessException("权限不足");
    }
}