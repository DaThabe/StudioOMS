using StudioOMS.Employees;

namespace StudioOMS.Users;


public sealed class User : Entity<UserId>
{
    public required string Username { get; init; }
    public string PasswordHash { get; private set; }
    public required EmployeeId EmployeeId { get; init; }



    public void ChangePassword(string hash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hash);
        var trimmed = hash.Trim();

        // 相同
        if (string.Equals(trimmed, Username, StringComparison.OrdinalIgnoreCase))
            return;

        PasswordHash = trimmed;
    }


    internal User(string passwordHash) => PasswordHash = passwordHash;
    public static User Create(UserId userId, string username, string passwordHash, EmployeeId employeeId)
    {
        if (userId == UserId.Empty)
            throw new ArgumentException("用户 Id 不可为空", nameof(userId));

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("涌户名不可为空", nameof(username));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("密码哈希不可为空", nameof(passwordHash));

        if (employeeId == EmployeeId.Empty)
            throw new ArgumentException("员工 Id 不可为空", nameof(employeeId));


        return new(passwordHash)
        {
            Id = userId,
            Username = username,
            EmployeeId = employeeId
        };
    }
    public static User Create(string username, string passwordHash, EmployeeId employeeId) =>
        Create(UserId.Create(), username, passwordHash, employeeId);
}