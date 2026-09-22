using StudioOMS.Users;

namespace StudioOMS.Session;


public interface ISessionInfoRepository
{
    ValueTask SaveAsync(SessionToken token, SessionInfo info, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据令牌查询信息
    /// </summary>
    ValueTask<SessionInfo?> FindAsync(SessionToken token, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除此会话信息
    /// </summary>
    ValueTask RemoveAsync(SessionToken token, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除此用户的会话信息
    /// </summary>
    ValueTask RemoveByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
}