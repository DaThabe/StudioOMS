namespace StudioOMS.Login;


public readonly record struct LoginDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public readonly record struct LoginResult
{
    public required string Token { get; init; }
}