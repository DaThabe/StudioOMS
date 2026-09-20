namespace StudioOMS.Security;


internal sealed class PasswordHasher : IPasswordHasher
{
    public ValueTask<string> HashAsync(string rawPassword, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rawPassword);

        var hash = BCrypt.Net.BCrypt.HashPassword(rawPassword);
        return ValueTask.FromResult(hash);
    }

    public ValueTask<bool> VerifyAsync(string rawPassword, string passwordHash, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rawPassword);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        var isValid = BCrypt.Net.BCrypt.Verify(rawPassword, passwordHash);
        return ValueTask.FromResult(isValid);
    }
}