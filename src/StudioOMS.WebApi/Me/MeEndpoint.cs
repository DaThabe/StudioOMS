using StudioOMS.Messaging;

namespace StudioOMS.Me;


public static class MeEndpoint
{
    public static async Task<IResult> InfoAsync(
        ISender sender,
        CancellationToken ct)
    {
        var response = await sender.SendAsync<MineInfoRequest, MineInfoResponse>(new MineInfoRequest(), ct);
        return Results.Ok(response.ToInfoResult());
    }

    public static async Task<IResult> ChangePasswordAsync(ChangePasswordDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(dto.ToRequest(), ct);
        return Results.Ok();
    }
}