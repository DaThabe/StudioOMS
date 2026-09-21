using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Login;
using StudioOMS.Me;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Security.Session;
using StudioOMS.Users;


#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddStudioOMSHandlers()
        {
            // User
            services.AddRequestHandler<UserCreateRequest.Handler, UserCreateRequest, UserId>();
            services.AddRequestHandler<LoginRequest.Handler, LoginRequest, SessionToken>();
            services.AddRequestHandler<ChangePasswordRequest.Handler, ChangePasswordRequest>();

            // Employee
            services.AddRequestHandler<EmployeeCreateRequest.Handler, EmployeeCreateRequest, EmployeeId>();

            // Customer
            services.AddRequestHandler<CustomerCreateRequest.Handler, CustomerCreateRequest, CustomertId>();

            // Order
            services.AddRequestHandler<OrderAssignEmployeeRequest.Handler, OrderAssignEmployeeRequest>();
            services.AddRequestHandler<OrderMarkServicingRequest.Handler, OrderMarkServicingRequest>();
            services.AddRequestHandler<OrderListRequest.Handler, OrderListRequest, OrderListResult>();
            //Order-Timing
            services.AddRequestHandler<TimingOrderCreateRequest.Handler, TimingOrderCreateRequest, OrderId>();
            services.AddRequestHandler<TimingOrderConsumeRequest.Handler, TimingOrderConsumeRequest>();
        }



        private IServiceCollection AddRequestHandler<THandler, TRequest>()
            where THandler : class, IRequestHandler<TRequest>
            where TRequest : IRequest
        {
            return services.AddScoped<IRequestHandler<TRequest>, THandler>();
        }
        private IServiceCollection AddRequestHandler<THandler, TRequest, TResponse>()
            where THandler : class, IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            return services.AddScoped<IRequestHandler<TRequest, TResponse>, THandler>();
        }
    }
}
