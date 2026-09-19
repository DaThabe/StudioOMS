using StudioOMS.Demo.Order.Timing;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Demo;


public sealed class StudioOMSClient(ISender sender)
{
    public async ValueTask<TimingOrderClient> CreateTimingOrderAsync(ClientId clientId, EmployeeId salespersonId, decimal totalDays)
    {
        var request = new TimingOrderCreateRequest(
            OrderId: OrderId.Create(),
            ClientId: clientId,
            SalespersonId: salespersonId,
            TotalDays: totalDays,
            CreateAt: DateTime.UtcNow
        );

        var orderId = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(request);
        return new(sender, orderId);
    }
}