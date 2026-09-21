using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS;

public interface IOrderClient
{
    ValueTask AssignEmployeeAsync(OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default);
    ValueTask<OrderListResult> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default);


    ValueTask<OrderCreateResult> CreateTimingAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken = default);
    ValueTask ConsumeTimingAsync(TimingOrderConsume dto, CancellationToken cancellationToken = default);
}