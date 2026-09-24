using StudioOMS.Serializer;
using StudioOMS.Users;
using StudioOMS.WebApi.Routes;

namespace StudioOMS.WebApi.Clients;

public interface IUserClient
{
    Task<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);
}

internal sealed class UserClient(UserRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IUserClient
{
    public async Task<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Users, dto, DtoJsonSerializerContext.Default.UserCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, DtoJsonSerializerContext.Default.UserCreateResult, cancellationToken);
    }
}