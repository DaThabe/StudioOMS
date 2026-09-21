using StudioOMS.Http;
using StudioOMS.Me;
using StudioOMS.Routes;
using StudioOMS.Serializer;

namespace StudioOMS.Clients;


public interface IMeClient
{
    Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken = default);
}

internal sealed class MeClient(MeRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IMeClient
{
    public async Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Password, dto, DtoJsonSerializerContext.Default.ChangePasswordDto);
        messageOptionsAction?.Invoke(request);

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}