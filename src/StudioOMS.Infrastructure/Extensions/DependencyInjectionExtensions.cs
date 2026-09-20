using Microsoft.EntityFrameworkCore;
using StudioOMS;
using StudioOMS.Clients;
using StudioOMS.EfCore;
using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Repositories;
using StudioOMS.Users;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        [RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
        [RequiresDynamicCode("EF Core isn't fully compatible with NativeAOT, and running the application may generate unexpected runtime failures.")]
        public IServiceCollection AddInfrastructure(Action<DbContextOptionsBuilder> databaseOptionAction)
        {
            return services
                .AddDatabase(databaseOptionAction)
                .AddRepository()
                .AddMessaging()
                .AddPassword();
        }


        [RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
        [RequiresDynamicCode("EF Core isn't fully compatible with NativeAOT, and running the application may generate unexpected runtime failures.")]

        public IServiceCollection AddDatabase(Action<DbContextOptionsBuilder> buildAction)
        {
            services.AddDbContext<AppDbContext>(buildAction);
            services.AddHostedService<MigrateHostedService>();
            return services;
        }

        public IServiceCollection AddRepository()
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }


        public IServiceCollection AddMessaging()
        {
            services.AddScoped<ISender, Sender>();
            return services;
        }

        public IServiceCollection AddPassword()
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
