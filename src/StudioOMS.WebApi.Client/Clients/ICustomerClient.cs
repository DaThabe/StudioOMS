using StudioOMS.Customers;
using StudioOMS.Extensions;
using StudioOMS.Routes;
using StudioOMS.Serializer;

namespace StudioOMS.Clients;


public interface ICustomerClient
{
    Task<CustomerCreateResult> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default);
}

internal sealed class CustomerClient(CustomerRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : ICustomerClient
{
    public async Task<CustomerCreateResult> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Create, dto, AppJsonSerializerContext.Default.CustomerCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, AppJsonSerializerContext.Default.CustomerCreateResult, cancellationToken);
    }
}