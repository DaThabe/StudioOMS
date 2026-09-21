using Microsoft.Extensions.DependencyInjection;
using StudioOMS.Exceptions;
using StudioOMS.Messaging;
using StudioOMS.Security.Permission;
using StudioOMS.Security.Session;

namespace StudioOMS;


internal sealed class MessageSender(IServiceProvider services, ICurrentSession currentSession, IPermissionChecker permissionChecker) : ISender
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
        // 不需要认证
        if (handler is IAllowAnonymous) return;

        // 需要认证
        if (handler is IAuthentication)
        {
            // 检查认证
            var currentEmployeeId = currentSession.Info?.EmployeeId;
            NotAuthenticatedException.ThrowIf(currentEmployeeId is null);

            // 需要授权
            if (handler is IAuthorization authorization)
            {
                // 检查授权
                var pass = await permissionChecker.HasAllPermissionsAsync(currentEmployeeId.Value, authorization.RequiredPermissions);
                ForbiddenException.ThrowIf(!pass);
            }

            return;
        }

        throw new InvalidOperationException($"处理器 [{handler}] 配置错误, 未设置约束信息");
    }
}