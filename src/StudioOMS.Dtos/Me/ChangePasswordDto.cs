namespace StudioOMS.Me;


public readonly record struct ChangePasswordDto
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
}