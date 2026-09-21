using Microsoft.EntityFrameworkCore;
using StudioOMS;
using StudioOMS.Customers;
using StudioOMS.EfCore;
using StudioOMS.Employees;
using StudioOMS.Me;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Queries;
using StudioOMS.Repositories;
using StudioOMS.Security;
using StudioOMS.Security.Permission;
using StudioOMS.Security.Session;
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
                .AddMemoryCache()
                .AddDatabase(databaseOptionAction)
                .AddRepositories()
                .AddQueries()
                .AddMessaging()
                .AddSecurity();
        }


        [RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
        [RequiresDynamicCode("EF Core isn't fully compatible with NativeAOT, and running the application may generate unexpected runtime failures.")]

        public IServiceCollection AddDatabase(Action<DbContextOptionsBuilder> buildAction)
        {
            services.AddDbContext<AppDbContext>(buildAction);
            services.AddHostedService<MigrateHostedService>();
            services.AddHostedService<InitAdminHostedService>();
            return services;
        }

        public IServiceCollection AddRepositories()
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
        public IServiceCollection AddQueries()
        {
            services.AddScoped<IOrderQuery, OrderQuery>();
            services.AddScoped<IMineInfoQuery, MineInfoQuery>();

            return services;
        }


        public IServiceCollection AddMessaging()
        {
            services.AddScoped<ISender, MessageSender>();
            services.AddScoped<ICurrentSession, CurrentSession>();
            return services;
        }

        public IServiceCollection AddSecurity()
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddScoped<IPermissionChecker, PermissionChecker>();

            return services;
        }
    }
}
