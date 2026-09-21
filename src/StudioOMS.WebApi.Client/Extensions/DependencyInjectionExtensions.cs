using StudioOMS;
using System.Net.Http.Headers;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddStudioOMSClient(Uri defaultBaseUrl)
        {
            return services.AddSingleton<IStudioOMSClientFactory>(_ => new StudioOMSClientFactory(defaultBaseUrl));
        }

        public IServiceCollection AddStudioOMSClient(Uri baseUrl, AuthenticationHeaderValue authentication)
        {
            services.AddSingleton<IStudioOMSClient>(_ => new StudioOMSClient(baseUrl, authentication));
            services.AddSingleton<IStudioOMSClientFactory>(_ => new StudioOMSClientFactory(baseUrl));

            return services;
        }
    }
}
