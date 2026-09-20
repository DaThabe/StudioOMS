namespace StudioOMS.Users;


public readonly record struct UserCreateDto
{
    public required Guid Id { get; init; }
    public required string Password { get; init; }
    public required string Name { get; init; }
    public required Guid EmployeeId { get; init; }
}

public readonly record struct UserCreateResult
{
    public required Guid UserId { get; init; }
}