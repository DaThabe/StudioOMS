using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Demo.Order.Timing;

public sealed class TimingOrderClient(ISender sender, OrderId orderId) : OrderClient(sender, orderId)
{
    private ISender _sender = sender;
    private OrderId _orderId = orderId;


    public async ValueTask<TimingOrderConsumeResult> ConsumeAsync(EmployeeId employeeId, decimal consuemDays)
    {
        var request = new TimingOrderConsumeRequest(
            OrderId: _orderId,
            EmployeeId: employeeId,
            ConsumeId: ConsumeId.Create(),
            ConsuemDays: consuemDays,
            Timestamp: DateTime.UtcNow
        );

        return await _sender.SendAsync<TimingOrderConsumeRequest, TimingOrderConsumeResult>(request);
    }
}