using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Permission;
using StudioOMS.Session;

namespace StudioOMS.Orders;


internal interface IOrderMarkRequest : IRequest
{
    OrderId OrderId { get; }

    public abstract class Handler<TRequest>(
        ICurrentSession currentSession,
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository
    ) : IRequestHandler<TRequest>, IAuthorization
        where TRequest : IOrderMarkRequest
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.OrderManage);

        public async ValueTask HandleAsync(TRequest request,
            CancellationToken cancellationToken = default)
        {
            var employee = await employeeRepository.FindByIdAsync(currentSession.EmployeeId, cancellationToken)
                ?? throw new InvalidOperationException($"员工 {request.OrderId} 不存在");
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            HandleOrder(order, employee);

            await orderRepository.SaveAsync(order, cancellationToken);
        }

        protected abstract void HandleOrder(Order order, Employee employees);
    }
}


// 服务
public sealed class OrderMarkServicingRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(
        ICurrentSession currentSession,
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository
    ) : IOrderMarkRequest.Handler<OrderMarkServicingRequest>(
            currentSession,
            employeeRepository,
            orderRepository)
    {
        protected override void HandleOrder(Order order, Employee employees) =>
            OrderStatePolicy.MarkServicing(order, employees, DateTimeOffset.Now);
    }
}

// 暂停
public sealed class OrderMarkPausedRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(
        ICurrentSession currentSession,
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository
    ) : IOrderMarkRequest.Handler<OrderMarkPausedRequest>(
            currentSession,
            employeeRepository,
            orderRepository)
    {
        protected override void HandleOrder(Order order, Employee employees) =>
            OrderStatePolicy.MarkPaused(order, employees, DateTimeOffset.Now);
    }
}

// 取消
public sealed class OrderMarkCancelledRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(
        ICurrentSession currentSession,
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository
    ) : IOrderMarkRequest.Handler<OrderMarkCancelledRequest>(
            currentSession,
            employeeRepository,
            orderRepository)
    {
        protected override void HandleOrder(Order order, Employee employees) =>
            OrderStatePolicy.MarkCancelled(order, employees, DateTimeOffset.Now);
    }
}

// 终止
public sealed class OrderMarkTerminatedRequest : IOrderMarkRequest
{
    public required OrderId OrderId { get; init; }

    internal sealed class Handler(
         ICurrentSession currentSession,
         IEmployeeRepository employeeRepository,
         IOrderRepository orderRepository
     ) : IOrderMarkRequest.Handler<OrderMarkTerminatedRequest>(
             currentSession,
             employeeRepository,
             orderRepository)
    {
        protected override void HandleOrder(Order order, Employee employees) =>
            OrderStatePolicy.MarkTerminated(order, employees, DateTimeOffset.Now);
    }
}