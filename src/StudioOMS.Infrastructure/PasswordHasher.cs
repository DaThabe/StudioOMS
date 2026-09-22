using StudioOMS.Users;

namespace StudioOMS;


internal sealed class PasswordHasher : IPasswordHasher
{
    public ValueTask<string> HashAsync(Password rawPassword, CancellationToken cancellationToken = default)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(rawPassword.GetRawText());
        return ValueTask.FromResult(hash);
    }

    public ValueTask<bool> VerifyAsync(string rawPassword, string passwordHash, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        var isValid = BCrypt.Net.BCrypt.Verify(rawPassword, passwordHash);
        return ValueTask.FromResult(isValid);
    }
}