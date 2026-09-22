using StudioOMS.Users;

namespace StudioOMS;


public interface IPasswordHasher
{
    ValueTask<string> HashAsync(Password rawPassword, CancellationToken cancellationToken = default);
    ValueTask<bool> VerifyAsync(string rawPassword, string passwordHash, CancellationToken cancellationToken = default);
}