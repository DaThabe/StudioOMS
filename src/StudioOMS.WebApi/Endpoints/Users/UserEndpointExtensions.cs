using StudioOMS.Requests;
using StudioOMS.Requests.Users;
using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Endpoints.Users;


public static class UserEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapUserEndpoints()
        {
            var group = app.MapGroup("/api/users");

            group.MapPost("/", CreateAsync);
            group.MapPost("/login", LoginAsync);

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

    private static async Task<IResult> LoginAsync(UserLoginDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var token = await sender.SendAsync<UserLoginRequest, SessionToken>(UserLoginRequest.FromDto(dto), ct);
        return Results.Ok(token.ToString());
    }
}