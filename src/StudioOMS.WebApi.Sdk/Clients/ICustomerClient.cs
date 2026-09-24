using StudioOMS.Customers;
using StudioOMS.Serializer;
using StudioOMS.WebApi.Responses;
using StudioOMS.WebApi.Routes;
using StudioOMS.WebApi.Serializer;

namespace StudioOMS.WebApi.Clients;


public interface ICustomerClient
{
    Task<Response<CustomerCreateResult>> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default);
}

internal sealed class CustomerClient(CustomerRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : ICustomerClient
{
    public async Task<Response<CustomerCreateResult>> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Customers, dto, DtoJsonSerializerContext.Default.CustomerCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetResponseAsync(request, ResponseJsonSerializerContext.Default.ResponseCustomerCreateResult, cancellationToken);
    }
}