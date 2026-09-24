using StudioOMS.Customers;
using StudioOMS.Mappers;
using StudioOMS.Messaging;

namespace StudioOMS.Endpoints;


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