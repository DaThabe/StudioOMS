using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Orders.Timing;


public sealed record class TimingOrderCreateRequest : IRequest<OrderId>
{
    public required ClientId ClientId { get; init; }
    public required EmployeeId SalespersonId { get; init; }
    public required decimal TotalDays { get; init; }
    public string Title { get; init; } = "未命名的订单";


    public static TimingOrderCreateRequest FromDto(TimingOrderCreateDto dto)
    {
        return new()
        {
            ClientId = ClientId.Parse(dto.ClientId),
            SalespersonId = EmployeeId.Parse(dto.SalespersonId),
            TotalDays = dto.TotalDays,
            Title = dto.Title
        };
    }


    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<TimingOrderCreateRequest, OrderId>, IRequirePermissions
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } = PermissionType.Group(PermissionType.OrderCreate);

        public async ValueTask<OrderId> HandleAsync(TimingOrderCreateRequest request, CancellationToken cancellationToken = default)
        {
            var order = TimingOrder.CreateNow(request.ClientId, request.SalespersonId, request.TotalDays);
            await orderRepository.SaveAsync(order, cancellationToken);

            return order.Id;
        }
    }
}
