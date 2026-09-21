using StudioOMS.Messaging;
using StudioOMS.Security.Session;

namespace StudioOMS.Me;


public static class LoginEndpoint
{
    public static async Task<IResult> LoginAsync(LoginDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var response = await sender.SendAsync<LoginRequest, SessionToken>(dto.ToRequest(), ct);
        return Results.Ok(response.ToLoginResult());
    }
}