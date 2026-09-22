using StudioOMS.Users;

namespace StudioOMS.Session;


public interface ISessionManager
{
    /// <summary>
    /// 登记用户并创建 会话令牌
    /// </summary>
    ValueTask<SessionToken> SignInAsync(UserId userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 登出会话
    /// </summary>
    ValueTask SignOutAsync(SessionToken token, CancellationToken cancellationToken = default);

    /// <summary>
    /// 登出用户的所有会话
    /// </summary>
    ValueTask SignOutAllAsync(UserId userId, CancellationToken cancellationToken = default);
}