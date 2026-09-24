using StudioOMS.Mappers;
using StudioOMS.Messaging;
using StudioOMS.Users;

namespace StudioOMS.Endpoints;


public static class UserEndpoint
{
    public static async Task<IResult> CreateAsync(UserCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var response = await sender.SendAsync<UserCreateRequest, UserId>(dto.ToRequest(), ct);
        return Results.Ok(response.ToUserCreateResult());
    }
}
