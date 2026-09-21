using StudioOMS.Extensions;
using StudioOMS.Me;
using StudioOMS.Serializer;
using StudioOMS.Users;

namespace StudioOMS;

public interface IUserClient
{
    Task<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken = default);
}

internal sealed class UserClient(ServerUrl url, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IUserClient
{
    public async Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PutJson(url.UserPassword, dto, AppJsonSerializerContext.Default.ChangePasswordDto);
        messageOptionsAction?.Invoke(request);

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(url.Users, dto, AppJsonSerializerContext.Default.UserCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, AppJsonSerializerContext.Default.UserCreateResult, cancellationToken);
    }
}