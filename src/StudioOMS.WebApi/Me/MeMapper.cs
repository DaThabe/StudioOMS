using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Me;

public static class MeMapper
{
    public static LoginRequest ToRequest(this LoginDto dto)
    {
        return new()
        {
            Username = Username.From(dto.Username),
            Password = dto.Password
        };
    }
    public static LoginResult ToLoginResult(this SessionToken dto)
    {
        return new()
        {
            Token = dto.ToString()
        };
    }


    public static ChangePasswordRequest ToRequest(this ChangePasswordDto dto)
    {
        return new()
        {
            CurrentPassword = dto.CurrentPassword,
            NewPassword = Password.From(dto.NewPassword)
        };
    }
}
