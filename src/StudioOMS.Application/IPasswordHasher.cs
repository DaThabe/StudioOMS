namespace StudioOMS;


public interface IPasswordHasher
{
    ValueTask<string> HashAsync(string rawPassword, CancellationToken cancellationToken = default);
    ValueTask<bool> VerifyAsync(string rawPassword, string passwordHash, CancellationToken cancellationToken = default);
}