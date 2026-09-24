using StudioOMS.WebApi.Responses;
using StudioOMS.WebApi.Routes;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace System.Net.Http;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


internal static class HttpExtensions
{
    extension(HttpClient httpClient)
    {
        public async Task SendEnsureSuccessAsync(
            HttpRequestMessage requestMessage,
            CancellationToken cancellationToken = default)
        {
            var response = await httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<TResponse?> GetJsonAsync<TResponse>(
            HttpRequestMessage requestMessage,
            JsonTypeInfo<TResponse> jsonTypeInfo,
            CancellationToken cancellationToken = default)
            where TResponse : notnull
        {
            var response = await httpClient.SendAsync(requestMessage, cancellationToken);
            return await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
        }

        public async Task<Response<TData>> GetResponseAsync<TData>(
            HttpRequestMessage requestMessage,
            JsonTypeInfo<Response<TData>> jsonTypeInfo,
            CancellationToken cancellationToken = default)
            where TData : notnull
        {
            return await httpClient.GetJsonAsync(requestMessage, jsonTypeInfo, cancellationToken);
        }



        public ServerRoutes GetServerRoutes()
        {
            if (httpClient.BaseAddress is null)
            {
                throw new ArgumentException(
                    "HttpClient.BaseAddress 未设置。请通过 AddHttpClient(StudioOMSClientNames.Default, c => c.BaseAddress = ...) 配置默认 baseUrl。",
                    nameof(httpClient));
            }

            return new ServerRoutes(httpClient.BaseAddress);
        }
    }

    extension(HttpRequestMessage)
    {
        public static HttpRequestMessage Request(HttpMethod method, string requesturi, Action<HttpRequestMessage>? optionsAction = null)
        {
            var message = new HttpRequestMessage(method, requesturi);
            optionsAction?.Invoke(message);
            return message;
        }
        public static HttpRequestMessage Post(string requesturi, Action<HttpRequestMessage>? optionsAction = null) =>
            Request(HttpMethod.Post, requesturi, optionsAction);
        public static HttpRequestMessage Get(string requesturi, Action<HttpRequestMessage>? optionsAction = null) =>
            Request(HttpMethod.Get, requesturi, optionsAction);
        public static HttpRequestMessage Put(string requesturi, Action<HttpRequestMessage>? optionsAction = null) =>
            Request(HttpMethod.Put, requesturi, optionsAction);


        public static HttpRequestMessage PutJson<T>(string requesturi, T data, JsonTypeInfo<T> jsonTypeInfo) =>
            Put(requesturi, x => x.Content = JsonContent.Create(data, jsonTypeInfo));
        public static HttpRequestMessage PostJson<T>(string requesturi, T data, JsonTypeInfo<T> jsonTypeInfo) =>
            Post(requesturi, x => x.Content = JsonContent.Create(data, jsonTypeInfo));
    }
}
