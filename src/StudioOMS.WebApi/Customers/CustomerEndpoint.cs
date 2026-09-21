using StudioOMS.Messaging;

namespace StudioOMS.Customers;


internal static class CustomerEndpoint
{
    public static async Task<IResult> CreateAsync(CustomerCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<CustomerCreateRequest, CustomertId>(dto.ToRequest(), ct);
        return Results.Ok(new CustomerCreateResult() { Id = id.ToString() });
    }
}