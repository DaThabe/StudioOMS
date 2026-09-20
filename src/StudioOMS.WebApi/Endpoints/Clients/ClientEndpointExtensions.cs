using StudioOMS.Clients;
using StudioOMS.Requests;
using StudioOMS.Requests.Clients;

namespace StudioOMS.Endpoints.Clients;


public static class ClientEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapClientEndpoints()
        {
            var group = app.MapGroup("/api/clients");

            group.MapPost("/", CreateAsync);

            return app;
        }
    }

    private static async Task<IResult> CreateAsync(ClientCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<ClientCreateRequest, ClientId>(ClientCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
}