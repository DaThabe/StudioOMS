using StudioOMS.Me;
using StudioOMS.Serializer;
using StudioOMS.WebApi.Responses;
using StudioOMS.WebApi.Routes;
using StudioOMS.WebApi.Serializer;

namespace StudioOMS.WebApi;

public interface IStudioOMSAuthentication
{
    Task<Response<LoginResult>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task<Response<LoginResult>> LoginAsync(LoginDto dto, Uri baseUrl, CancellationToken cancellationToken = default);
}

internal sealed class StudioOMSAuthentication(IHttpClientFactory factory) : IStudioOMSAuthentication
{
    public Task<Response<LoginResult>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var httpClient = factory.CreateClient("StudioOMS");
        var routes = httpClient.GetServerRoutes();

        return LoginAsync(httpClient, routes, dto, cancellationToken);
    }

    public Task<Response<LoginResult>> LoginAsync(LoginDto dto, Uri baseUrl, CancellationToken cancellationToken = default)
    {
        var routes = new ServerRoutes(baseUrl);
        var httpClient = factory.CreateClient();

        return LoginAsync(httpClient, routes, dto, cancellationToken);
    }


    private static async Task<Response<LoginResult>> LoginAsync(HttpClient httpClient, ServerRoutes routes, LoginDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Login, dto, DtoJsonSerializerContext.Default.LoginDto);
        return await httpClient.GetResponseAsync(request, ResponseJsonSerializerContext.Default.ResponseLoginResult, cancellationToken);
    }
}