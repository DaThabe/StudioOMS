using StudioOMS.Customers;
using StudioOMS.Messaging;
using StudioOMS.WebApi.Mappers;

namespace StudioOMS.WebApi.Endpoints;


internal static class CustomerEndpoint
{
    public static async Task<IResult> CreateAsync(CustomerCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<CustomerCreateRequest, CustomertId>(dto.ToRequest(), ct);
        return ResponseResults.Ok(new CustomerCreateResult() { CustomerId = id.ToString() });
    }
}