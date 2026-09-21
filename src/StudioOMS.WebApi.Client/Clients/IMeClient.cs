using StudioOMS.Http;
using StudioOMS.Me;
using StudioOMS.Routes;
using StudioOMS.Serializer;

namespace StudioOMS.Clients;


public interface IMeClient
{
    Task<InfoResult> GetInfoAsync(CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken = default);
}

internal sealed class MeClient(MeRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IMeClient
{
    public Task<InfoResult> GetInfoAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Get(routes.Info);
        messageOptionsAction?.Invoke(request);

        return client.GetJsonAsync(request, DtoJsonSerializerContext.Default.InfoResult, cancellationToken);
    }

    public Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Password, dto, DtoJsonSerializerContext.Default.ChangePasswordDto);
        messageOptionsAction?.Invoke(request);

        return client.SendEnsureSuccessAsync(request, cancellationToken);
    }
}