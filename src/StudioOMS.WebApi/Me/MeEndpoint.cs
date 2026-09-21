using StudioOMS.Messaging;

namespace StudioOMS.Me;


public static class MeEndpoint
{
    public static async Task<IResult> ChangePasswordAsync(ChangePasswordDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(dto.ToRequest(), ct);
        return Results.Ok();
    }
}