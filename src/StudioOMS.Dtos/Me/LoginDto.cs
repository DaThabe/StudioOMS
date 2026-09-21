using System.ComponentModel.DataAnnotations;

namespace StudioOMS.Me;


public readonly record struct LoginDto
{
    [Required(ErrorMessage = "请输入用户名")]
    public required string Username { get; init; }

    [Required(ErrorMessage = "请输入密码")]
    public required string Password { get; init; }
}

public readonly record struct LoginResult
{
    public required string Token { get; init; }
}