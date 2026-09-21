using StudioOMS.Extensions;
using StudioOMS.Serializer;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

namespace StudioOMS;


public sealed record class StudioOMSAuthorization : IEquatable<StudioOMSAuthorization>
{
    public required string Scheme { get; init; }
    public required string Param { get; init; }


    private StudioOMSAuthorization() { }
    public override string ToString() => $"{Scheme} {Scheme[..3]}...{Scheme[^3..]}";

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(string.GetHashCode(Scheme, StringComparison.OrdinalIgnoreCase));
        hashCode.Add(Param);

        return hashCode.ToHashCode();
    }
    public bool Equals(StudioOMSAuthorization? other)
    {
        if (!string.Equals(Scheme, other?.Scheme, StringComparison.OrdinalIgnoreCase)) return false;
        if (!string.Equals(Param, other?.Param)) return false;
        return true;
    }




    public static StudioOMSAuthorization Create(string scheme, string param)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scheme);
        ArgumentException.ThrowIfNullOrWhiteSpace(param);

        return new StudioOMSAuthorization()
        {
            Scheme = scheme.Trim(),
            Param = param.Trim()
        };
    }
}


public interface IHttpClient
{
    ValueTask<TResponse> PostJsonAsync<TPostData, TResponse>(
        string url,
        TPostData postData,
        JsonTypeInfo<TPostData> requestJsonTypeInfo,
        JsonTypeInfo<TResponse> responseJsonTypeInfo,
        CancellationToken cancellationToken = default);
}



public sealed class AuthorizationStudioOMSClient : IHttpClient
{
    private AuthenticationHeaderValue _authorization;
    private AuthorizationStudioOMSClient(StudioOMSAuthorization authorization) =>
        _authorization = new(authorization.Scheme, authorization.Param);




    public async ValueTask<TResponse?> PostJsonAsync<TPostData, TResponse>(string url, TPostData postData, JsonTypeInfo<TPostData> postDataJsonTypeInfo, JsonTypeInfo<TResponse> responseJsonTypeInfo, CancellationToken cancellationToken = default)
    {
        IStudioOMSClient clien;


        var requestMessage = RequestJsonContentMessage(HttpMethod.Post, url, postData, postDataJsonTypeInfo);
        return await requestMessage.Content?.ReadFromJsonAsync(responseJsonTypeInfo, cancellationToken);

    }

    private HttpRequestMessage RequestJsonContentMessage<TContent>(HttpMethod method, string url, TContent content, JsonTypeInfo<TContent> contentJsonTypeInfo)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = _authorization;
        request.Content = JsonContent.Create(content, contentJsonTypeInfo);

        return request;
    }



    public static AuthorizationStudioOMSClient WithAuthorization(StudioOMSAuthorization authorization)
    {
        return new AuthorizationStudioOMSClient(authorization);
    }

    public static async ValueTask<AuthorizationStudioOMSClient> LoginAsync(ServiceUrl serviceUrl, LoginDto dto, CancellationToken cancellationToken = default)
    {
        var loginResult = await _sharedHttpClient.PostJsonAsync(
            serviceUrl.UserLoginPath,
            dto,
            AppJsonSerializerContext.Default.LoginDto,
            AppJsonSerializerContext.Default.LoginResult,
            cancellationToken);

        var authorization = StudioOMSAuthorization.Create("Bearer", loginResult.Token);
        return WithAuthorization(authorization);
    }


    private static readonly HttpClient _sharedHttpClient = new();
}
