using StudioOMS.Customers;
using StudioOMS.Requests;
using StudioOMS.Requests.Customers;

namespace StudioOMS.Endpoints;


public static class CustomerEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapCustomerEndpoints()
        {
            var group = app.MapGroup("/api/customers");

            group.MapPost("/", CreateAsync);

            return app;
        }
    }

    private static async Task<IResult> CreateAsync(CustomerCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<CustomerCreateRequest, CustomertId>(CustomerCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
}