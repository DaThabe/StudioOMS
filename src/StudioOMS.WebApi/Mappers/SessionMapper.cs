using StudioOMS.Me;
using StudioOMS.Session;
using StudioOMS.Users;

namespace StudioOMS.WebApi.Mappers;


public static class SessionMapper
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
}
