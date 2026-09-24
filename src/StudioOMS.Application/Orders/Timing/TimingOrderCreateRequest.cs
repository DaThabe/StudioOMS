using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Permission;

namespace StudioOMS.Orders.Timing;


public sealed record class TimingOrderCreateRequest : IRequest<OrderId>
{
    public required CustomertId CustomerId { get; init; }
    public required EmployeeId SalespersonId { get; init; }
    public required Money Price { get; init; }
    public required decimal TotalDays { get; init; }
    public string Title { get; init; } = "未命名的订单";



    internal sealed class Handler(
            IOrderRepository orderRepository
        ) : IRequestHandler<TimingOrderCreateRequest, OrderId>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.OrderCreate);

        public async ValueTask<OrderId> HandleAsync(TimingOrderCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = TimingOrder.CreateNow(request.CustomerId, request.SalespersonId, request.Price, request.TotalDays);
            await orderRepository.SaveAsync(entity, cancellationToken);

            return entity.Id;
        }
    }
}