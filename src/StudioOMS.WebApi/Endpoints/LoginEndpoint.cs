using StudioOMS.Me;
using StudioOMS.Messaging;
using StudioOMS.Session;
using StudioOMS.WebApi.Mappers;

namespace StudioOMS.WebApi.Endpoints;


public static class LoginEndpoint
{
    public static async Task<IResult> LoginAsync(LoginDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var response = await sender.SendAsync<LoginRequest, SessionToken>(dto.ToRequest(), ct);
        return ResponseResults.Ok(response.ToLoginResult());
    }
}