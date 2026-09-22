using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Me;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Session;
using StudioOMS.Users;
using System.Diagnostics.CodeAnalysis;


#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddStudioOMSHandlers()
        {
            // Me
            services.AddRequestHandler<LoginRequest.Handler, LoginRequest, SessionToken>();
            services.AddRequestHandler<MineInfoRequest.Handler, MineInfoRequest, MeInfoResponse?>();
            services.AddRequestHandler<ChangePasswordRequest.Handler, ChangePasswordRequest>();

            // User
            services.AddRequestHandler<UserCreateRequest.Handler, UserCreateRequest, UserId>();

            // Employee
            services.AddRequestHandler<EmployeeCreateRequest.Handler, EmployeeCreateRequest, EmployeeId>();

            // Customer
            services.AddRequestHandler<CustomerCreateRequest.Handler, CustomerCreateRequest, CustomertId>();


            // Order
            services.AddRequestHandler<OrderAssignEmployeeRequest.Handler, OrderAssignEmployeeRequest>();
            services.AddRequestHandler<OrderListRequest.Handler, OrderListRequest, OrderListResponse>();
            // Order-State
            services.AddRequestHandler<OrderMarkServicingRequest.Handler, OrderMarkServicingRequest>();
            services.AddRequestHandler<OrderMarkPausedRequest.Handler, OrderMarkPausedRequest>();
            services.AddRequestHandler<OrderMarkCancelledRequest.Handler, OrderMarkCancelledRequest>();
            services.AddRequestHandler<OrderMarkTerminatedRequest.Handler, OrderMarkTerminatedRequest>();
            //Order-Timing
            services.AddRequestHandler<TimingOrderCreateRequest.Handler, TimingOrderCreateRequest, OrderId>();
            services.AddRequestHandler<TimingOrderConsumeRequest.Handler, TimingOrderConsumeRequest>();
        }



        private IServiceCollection AddRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
            where THandler : class, IRequestHandler<TRequest>
            where TRequest : IRequest
        {
            return services.AddScoped<IRequestHandler<TRequest>, THandler>();
        }
        private IServiceCollection AddRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
            where THandler : class, IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            return services.AddScoped<IRequestHandler<TRequest, TResponse>, THandler>();
        }
    }
}
