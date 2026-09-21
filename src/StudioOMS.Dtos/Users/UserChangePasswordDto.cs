namespace StudioOMS.Users;


public readonly record struct UserChangePasswordDto
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
}