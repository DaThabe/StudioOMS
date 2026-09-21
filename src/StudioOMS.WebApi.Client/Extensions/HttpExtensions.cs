using StudioOMS.Serializer;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

namespace StudioOMS.Extensions;

internal static class HttpExtensions
{
    extension(HttpClient client)
    {
        public async ValueTask<TResponse?> PostJsonAsync<TPostData, TResponse>(
            string url,
            TPostData data,
            JsonTypeInfo<TPostData> postDataJsonType,
            JsonTypeInfo<TResponse> responseJsonTypeInfo,
            CancellationToken cancellationToken = default)
        {
            var resp = await client.PostAsJsonAsync(url, data, postDataJsonType, cancellationToken);
            return await resp.Content.ReadFromJsonAsync(responseJsonTypeInfo, cancellationToken);
        }


        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public async ValueTask<TResponse?> PostJsonAsync<TResponse>(
           string url,
           object data,
           CancellationToken cancellationToken = default)
        {
            var resp = await client.PostAsJsonAsync(url, data, AppJsonSerializerContext.Default.Options, cancellationToken);
            return await resp.Content.ReadFromJsonAsync<TResponse>(AppJsonSerializerContext.Default.Options, cancellationToken);
        }

        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public async ValueTask PostJsonAsync(
           string url,
           object data,
           CancellationToken cancellationToken = default)
        {
            await client.PostAsJsonAsync(url, data, AppJsonSerializerContext.Default.Options, cancellationToken);
        }
    }
}
