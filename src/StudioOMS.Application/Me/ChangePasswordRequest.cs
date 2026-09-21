using StudioOMS.Messaging;
using StudioOMS.Security;
using StudioOMS.Security.Permission;
using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Me;


public sealed class ChangePasswordRequest : IRequest
{
    public required string CurrentPassword { get; init; }
    public required Password NewPassword { get; init; }


    internal sealed class Handler(
            ICurrentSession currentSession,
            ISessionService sessionService,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher
        ) : IRequestHandler<ChangePasswordRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.UserManage);

        public async ValueTask HandleAsync(ChangePasswordRequest request,
            CancellationToken cancellationToken = default)
        {
            var currentUserId = currentSession.Info?.UserId ??
                throw new InvalidOperationException("未登录");
            var entity = await userRepository.FindByIdAsync(currentUserId, cancellationToken)
                ?? throw new InvalidOperationException($"用户 {currentUserId} 不存在");

            // 检验旧密码
            var pass = await passwordHasher.VerifyAsync(request.CurrentPassword, entity.PasswordHash, cancellationToken);
            if (!pass) throw new InvalidOperationException("旧密码不匹配");

            // 创建新密码
            var newHashedPwd = await passwordHasher.HashAsync(request.NewPassword, cancellationToken);
            entity.SetPasswordHash(newHashedPwd);
            await userRepository.SaveAsync(entity, cancellationToken);

            // 删除会话信息
            await sessionService.RemoveByUserIdAsync(entity.Id, cancellationToken);
        }
    }
}