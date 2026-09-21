using StudioOMS.Employees;
using StudioOMS.Http;
using StudioOMS.Routes;
using StudioOMS.Serializer;

namespace StudioOMS.Clients;

public interface IEmployeeClient
{
    Task<EmployeeCreateResult?> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);
}


internal sealed class EmployeeClient(EmployeeRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IEmployeeClient
{
    public async Task<EmployeeCreateResult?> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Create, dto, DtoJsonSerializerContext.Default.EmployeeCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, DtoJsonSerializerContext.Default.EmployeeCreateResult, cancellationToken);
    }
}