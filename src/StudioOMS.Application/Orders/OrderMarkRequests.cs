using StudioOMS.Messaging;
using StudioOMS.Permission;

namespace StudioOMS.Orders;


internal interface IOrderMarkRequest : IRequest
{
    OrderId OrderId { get; }

    public abstract class Handler<TRequest>(IOrderRepository orderRepository) :
        IRequestHandler<TRequest>, IAuthorization
        where TRequest : IOrderMarkRequest
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.OrderManage);

        public async ValueTask HandleAsync(TRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            HandleOrder(entity);

            await orderRepository.SaveAsync(entity, cancellationToken);
        }

        protected abstract void HandleOrder(Order order);
    }
}


// 服务
public sealed class OrderMarkServicingRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(IOrderRepository orderRepository) :
        IOrderMarkRequest.Handler<OrderMarkServicingRequest>(orderRepository)
    {
        protected override void HandleOrder(Order order) =>
            order.MarkServicingNow();
    }
}

// 暂停
public sealed class OrderMarkPausedRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(IOrderRepository orderRepository) :
        IOrderMarkRequest.Handler<OrderMarkPausedRequest>(orderRepository)
    {
        protected override void HandleOrder(Order order) =>
            order.MarkPausedNow();
    }
}

// 取消
public sealed class OrderMarkCancelledRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(IOrderRepository orderRepository) :
        IOrderMarkRequest.Handler<OrderMarkCancelledRequest>(orderRepository)
    {
        protected override void HandleOrder(Order order) =>
            order.MarkCancelledNow();
    }
}

// 终止
public sealed class OrderMarkTerminatedRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(IOrderRepository orderRepository) :
        IOrderMarkRequest.Handler<OrderMarkTerminatedRequest>(orderRepository)
    {
        protected override void HandleOrder(Order order) =>
            order.MarkTerminatedNow();
    }
}