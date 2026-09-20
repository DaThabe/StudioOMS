using StudioOMS.Employees;

namespace StudioOMS.Users;


public sealed class User : Entity<UserId>
{
    public required string PasswordHash { get; init; }
    public required EmployeeId EmployeeId { get; init; }


    public string Name { get; private set; } = "未命名用户";



    public void ChangePassword(string hash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hash);
        var trimmed = hash.Trim();

        // 相同
        if (string.Equals(trimmed, Name, StringComparison.OrdinalIgnoreCase))
            return;

        Name = trimmed;
    }

    public void ChangeName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var trimmed = value.Trim();

        // 相同
        if (string.Equals(trimmed, Name, StringComparison.OrdinalIgnoreCase))
            return;

        Name = trimmed;
    }



    internal User() { }
    public static User Create(UserId userId, string passwordHash, EmployeeId employeeId)
    {
        if (userId == UserId.Empty)
            throw new ArgumentException("用户 Id 不可为空", nameof(userId));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("密码哈希不可为空", nameof(passwordHash));

        if (employeeId == EmployeeId.Empty)
            throw new ArgumentException("员工 Id 不可为空", nameof(employeeId));


        return new()
        {
            Id = userId,
            PasswordHash = passwordHash,
            EmployeeId = employeeId
        };
    }
    public static User Create(string passwordHash, EmployeeId employeeId) =>
        Create(UserId.Create(), passwordHash, employeeId);
}