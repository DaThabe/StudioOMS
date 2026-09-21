using StudioOMS.Security;
using StudioOMS.Security.Permission;
using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Requests.Users;


public sealed class UserChangePasswordRequest : IRequest
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }


    public static UserChangePasswordRequest FromDto(UserChangePasswordDto dto)
    {
        return new()
        {
            CurrentPassword = dto.CurrentPassword,
            NewPassword = dto.NewPassword
        };
    }

    internal sealed class Handler(
            ICurrentSession currentSession,
            ISessionService sessionService,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher
        ) : IRequestHandler<UserChangePasswordRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.UserManage);

        public async ValueTask HandleAsync(UserChangePasswordRequest request,
            CancellationToken cancellationToken = default)
        {
            var currentUserId = currentSession.Info?.UserId ??
                throw new InvalidOperationException("未登录");
            var user = await userRepository.FindByIdAsync(currentUserId, cancellationToken)
                ?? throw new InvalidOperationException($"用户 {currentUserId} 不存在");

            // 检验旧密码
            var pass = await passwordHasher.VerifyAsync(request.CurrentPassword, user.PasswordHash, cancellationToken);
            if (!pass) throw new InvalidOperationException("旧密码不匹配");

            // 创建新密码
            var newHashedPwd = await passwordHasher.HashAsync(request.NewPassword, cancellationToken);
            user.ChangePassword(newHashedPwd);
            await userRepository.SaveAsync(user, cancellationToken);

            // 删除会话信息
            await sessionService.RemoveByUserIdAsync(user.Id, cancellationToken);
        }
    }
}