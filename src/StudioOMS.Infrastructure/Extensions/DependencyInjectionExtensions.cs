using StudioOMS.Messaging;
using StudioOMS.Orders;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddMessagingSender()
        {
            services.AddScoped<ISender, Sender>();
        }

        public void AddStudioOMSRepository()
        {
            services.AddScoped<IOrderRepository, MemoryOrderRepository>();
        }
    }
}
