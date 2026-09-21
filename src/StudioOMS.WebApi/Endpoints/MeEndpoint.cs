using StudioOMS.Me;
using StudioOMS.Messaging;
using StudioOMS.Security.Session;

namespace StudioOMS.Endpoints;


public static class MeEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapMeEndpoints()
        {
            var group = app.MapGroup("/api/me");

            group.MapPost("/password", ChangePasswordAsync);
            return app;
        }
    }

    private static async Task<IResult> ChangePasswordAsync(ChangePasswordDto dto,
        ISender sender,
        ICurrentSession currentUser,
        CancellationToken ct)
    {
        await sender.SendAsync(ChangePasswordRequest.FromDto(dto), ct);
        return Results.Ok();
    }
}