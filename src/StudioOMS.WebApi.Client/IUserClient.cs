using StudioOMS.Users;

namespace StudioOMS;

public interface IUserClient
{
    ValueTask<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);
    ValueTask ChangePasswordAsync(UserChangePasswordDto dto, CancellationToken cancellationToken = default);
}
