using StudioOMS.Messaging;
using StudioOMS.Orders;

namespace StudioOMS.Demo.Order;

public class OrderClient(ISender sender, OrderId orderId)
{
    public ValueTask AssignEmployeeAsync(EmployeeId employeeId, CancellationToken cancellationToken = default)
    {
        var request = new OrderAssignEmployeeRequest(
            OrderId: orderId,
            EmployeeId: employeeId
        );

        return sender.SendAsync(request, cancellationToken);
    }

    public ValueTask<OrderStateChangeResult> MarkServicingAsync(CancellationToken cancellationToken = default)
    {
        var request = new OrderMarkServicingRequest(
            OrderId: orderId
        );

        return sender.SendAsync<OrderMarkServicingRequest, OrderStateChangeResult>(request, cancellationToken);
    }
}
