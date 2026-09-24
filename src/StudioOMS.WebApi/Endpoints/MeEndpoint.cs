using StudioOMS.Me;
using StudioOMS.Messaging;
using StudioOMS.WebApi.Mappers;

namespace StudioOMS.WebApi.Endpoints;


public static class MeEndpoint
{
    public static async Task<IResult> InfoAsync(
        ISender sender,
        CancellationToken ct)
    {
        var response = await sender.SendAsync<MeInfoRequest, MeInfoResponse>(new MeInfoRequest(), ct);
        return ResponseResults.Ok(response.ToInfoResult());
    }

    public static async Task<IResult> ChangePasswordAsync(ChangePasswordDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(dto.ToRequest(), ct);
        return ResponseResults.Ok();
    }
}