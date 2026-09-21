using StudioOMS.Extensions;
using StudioOMS.Routes;
using StudioOMS.Serializer;
using StudioOMS.Users;

namespace StudioOMS.Clients;

public interface IUserClient
{
    Task<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);
}

internal sealed class UserClient(UserRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IUserClient
{
    public async Task<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Create, dto, AppJsonSerializerContext.Default.UserCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, AppJsonSerializerContext.Default.UserCreateResult, cancellationToken);
    }
}