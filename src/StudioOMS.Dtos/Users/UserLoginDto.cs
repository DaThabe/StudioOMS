namespace StudioOMS.Users;


public readonly record struct UserLoginDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public readonly record struct UserLoginResult
{
    public required string Token { get; init; }
}