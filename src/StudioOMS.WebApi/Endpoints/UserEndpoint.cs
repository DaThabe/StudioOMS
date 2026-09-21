using StudioOMS.Messaging;
using StudioOMS.Users;

namespace StudioOMS.Endpoints;


public static class UserEndpoint
{
    public static async Task<IResult> CreateAsync(UserCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<UserCreateRequest, UserId>(UserCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
}