using StudioOMS.Customers;
using StudioOMS.Messaging;

namespace StudioOMS.Endpoints;


internal static class CustomerEndpoint
{
    public static async Task<IResult> CreateAsync(CustomerCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<CustomerCreateRequest, CustomertId>(CustomerCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
}