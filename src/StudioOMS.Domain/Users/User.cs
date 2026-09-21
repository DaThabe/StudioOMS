using StudioOMS.Employees;

namespace StudioOMS.Users;


public sealed class User : Entity<UserId>
{
    public required Username Username { get; init; }
    public string PasswordHash { get; private set; }
    public required EmployeeId EmployeeId { get; init; }



    public void SetPasswordHash(string hash)
    {
        VerificationPasswordHash(hash);
        if (hash == PasswordHash) return;

        // 更新
        PasswordHash = hash;
    }


    internal User(string passwordHash) => PasswordHash = passwordHash;
    public static User Create(UserId userId, Username username, string passwordHash, EmployeeId employeeId)
    {
        VerificationPasswordHash(passwordHash);

        return new(passwordHash)
        {
            Id = userId,
            Username = username,
            EmployeeId = employeeId
        };
    }
    public static User Create(Username username, string passwordHash, EmployeeId employeeId) =>
        Create(UserId.Create(), username, passwordHash, employeeId);



    private static void VerificationPasswordHash(ReadOnlySpan<char> hash)
    {
        if (hash.IsWhiteSpace())
            throw new ArgumentException("密码Hash不可为空", nameof(hash));
    }
}