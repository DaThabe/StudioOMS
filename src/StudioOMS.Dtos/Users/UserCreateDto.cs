namespace StudioOMS.Users;


public readonly record struct UserCreateDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string EmployeeId { get; init; }
}

public readonly record struct UserCreateResult
{
    public required string Id { get; init; }
}