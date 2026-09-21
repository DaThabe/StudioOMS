using StudioOMS.Requests;
using StudioOMS.Requests.Login;
using StudioOMS.Requests.Users;
using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Endpoints;


public static class UserEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapUserEndpoints()
        {
            var group = app.MapGroup("/api/users");

            group.MapPost("/", CreateAsync);
            group.MapPost("/me/password", ChangePasswordAsync);

            return app;
        }
    }

    private static async Task<IResult> CreateAsync(UserCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<UserCreateRequest, UserId>(UserCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }

    private static async Task<IResult> ChangePasswordAsync(UserChangePasswordDto dto,
        ISender sender,
        ICurrentSession currentUser,
        CancellationToken ct)
    {
        await sender.SendAsync(UserChangePasswordRequest.FromDto(dto), ct);
        return Results.Ok();
    }
}