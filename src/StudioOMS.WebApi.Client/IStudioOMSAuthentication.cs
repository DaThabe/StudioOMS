using StudioOMS.Http;
using StudioOMS.Me;
using StudioOMS.Routes;
using StudioOMS.Serializer;

namespace StudioOMS;

public interface IStudioOMSAuthentication
{
    Task<LoginResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginAsync(LoginDto dto, Uri baseUrl, CancellationToken cancellationToken = default);
}

internal sealed class StudioOMSAuthentication(IHttpClientFactory factory) : IStudioOMSAuthentication
{
    public Task<LoginResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var httpClient = factory.CreateClient("StudioOMS");
        var routes = httpClient.GetServerRoutes();

        return LoginAsync(httpClient, routes, dto, cancellationToken);
    }

    public Task<LoginResult> LoginAsync(LoginDto dto, Uri baseUrl, CancellationToken cancellationToken = default)
    {
        var routes = new ServerRoutes(baseUrl);
        var httpClient = factory.CreateClient();

        return LoginAsync(httpClient, routes, dto, cancellationToken);
    }


    private async Task<LoginResult> LoginAsync(HttpClient httpClient, ServerRoutes routes, LoginDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Login, dto, DtoJsonSerializerContext.Default.LoginDto);
        return await httpClient.GetJsonAsync(request, DtoJsonSerializerContext.Default.LoginResult, cancellationToken);
    }
}