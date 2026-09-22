using StudioOMS;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddStudioOMSClient(Uri baseUrl)
        {
            services.AddHttpClient(StudioOMSClientNames.Default, client =>
                client.BaseAddress = baseUrl);

            services.AddSingleton<IStudioOMSAuthentication, StudioOMSAuthentication>();
            services.AddSingleton<IStudioOMSClientFactory, StudioOMSClientFactory>();

            return services;
        }
    }
}
