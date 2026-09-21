using StudioOMS.Requests;
using StudioOMS.Requests.Login;
using StudioOMS.Security.Session;

namespace StudioOMS.Endpoints;


public static class LoginEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapLoginEndpoints()
        {
            app.MapPost("/api/login", LoginAsync);
            return app;
        }
    }

    private static async Task<IResult> LoginAsync(LoginDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var token = await sender.SendAsync<LoginRequest, SessionToken>(LoginRequest.FromDto(dto), ct);

        return Results.Ok(new LoginResult()
        {
            Token = token.ToString()
        });
    }
}