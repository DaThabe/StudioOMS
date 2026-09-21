using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

namespace StudioOMS.Extensions;


internal static class HttpExtensions
{
    extension(HttpClient client)
    {
        public async ValueTask<TResponse?> GetJsonAsync<TResponse>(
            HttpRequestMessage requestMessage,
            JsonTypeInfo<TResponse> jsonTypeInfo,
            CancellationToken cancellationToken = default)
        {
            var response = await client.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken);
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
