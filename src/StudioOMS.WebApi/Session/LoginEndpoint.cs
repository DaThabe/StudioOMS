using StudioOMS.Me;
using StudioOMS.Messaging;

namespace StudioOMS.Session;


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