using StudioOMS.Routes;
using System.Net.Http.Headers;

namespace StudioOMS;

public interface IStudioOMSClientFactory
{
    IStudioOMSClient CreateWithAuthentication(AuthenticationHeaderValue authentication);
    IStudioOMSClient CreateWithAuthentication(AuthenticationHeaderValue authentication, Uri baseUrl);
}


internal sealed class StudioOMSClientFactory(IHttpClientFactory httpClientFactory) : IStudioOMSClientFactory
{
    public IStudioOMSClient CreateWithAuthentication(AuthenticationHeaderValue authentication)
    {
        var httpClient = httpClientFactory.CreateClient(StudioOMSClientNames.Default);
        return StudioOMSClient.Create(httpClient, authentication);
    }

    public IStudioOMSClient CreateWithAuthentication(AuthenticationHeaderValue authentication, Uri baseUrl)
    {
        var httpClient = httpClientFactory.CreateClient();
        var serverRoutes = new ServerRoutes(baseUrl);

        return new StudioOMSClient(httpClient, serverRoutes, authentication);
    }
}
