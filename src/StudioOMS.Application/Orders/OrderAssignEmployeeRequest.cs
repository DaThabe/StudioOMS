using StudioOMS.Employees;
using StudioOMS.Messaging;

namespace StudioOMS.Orders;


public sealed record class OrderAssignEmployeeRequest : IRequest
{
    public required OrderId OrderId { get; init; }
    public required EmployeeId EmployeeId { get; init; }


    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<OrderAssignEmployeeRequest>
    {
        public async ValueTask HandleAsync(OrderAssignEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            order.AssignEmployees(request.EmployeeId);

            await orderRepository.SaveAsync(order, cancellationToken);
        }
    }
}
