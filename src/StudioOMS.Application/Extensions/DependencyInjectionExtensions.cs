using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddStudioOMSHandlers()
        {
            // Order
            services.AddRequestHandler<OrderAssignEmployeeRequest.Handler, OrderAssignEmployeeRequest>();
            services.AddRequestHandler<OrderMarkServicingRequest.Handler, OrderMarkServicingRequest, OrderStateChangeResult>();
            //Order-Timing
            services.AddRequestHandler<TimingOrderCreateRequest.Handler, TimingOrderCreateRequest, OrderId>();
            services.AddRequestHandler<TimingOrderConsumeRequest.Handler, TimingOrderConsumeRequest, ConsumeResult>();
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
