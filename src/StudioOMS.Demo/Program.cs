using Microsoft.Extensions.DependencyInjection;
using StudioOMS;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

// Services
var descriptors = new ServiceCollection();
descriptors.AddMessagingSender();
descriptors.AddStudioOMSHandlers();
descriptors.AddStudioOMSRepository();

// Handler
var services = descriptors.BuildServiceProvider();
var sender = services.GetRequiredService<ISender>();


// 创建订单
var totalDays = 10;

var timingOrderCreateRequest = new TimingOrderCreateRequest(OrderId.Create(), ClientId.Create(), EmployeeId.Create(), totalDays);
var orderId = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(timingOrderCreateRequest);

// 分配设计师
var designerId = EmployeeId.Create();
var orderAssignEmployeeRequest = new OrderAssignEmployeeRequest(orderId, designerId);
await sender.SendAsync(orderAssignEmployeeRequest);

// 订单开始服务
var orderMarkServicingRequest = new OrderMarkServicingRequest(orderId);
var markServicingResult = await sender.SendAsync<OrderMarkServicingRequest, OrderStateChangeResult>(orderMarkServicingRequest);
Console.WriteLine($"标记开始结果: {markServicingResult}");

// 开始消耗
for (int i = 0; i < totalDays; i++)
{
    var timingOrderConsumeRequest = new TimingOrderConsumeRequest(orderId, designerId, ConsumeId.Create(), 1);
    var consumeResult = await sender.SendAsync<TimingOrderConsumeRequest, ConsumeResult>(timingOrderConsumeRequest);

    Console.WriteLine($"划扣结果: {consumeResult}");
}

// 超额消耗
var exceedRequest = new TimingOrderConsumeRequest(orderId, designerId, ConsumeId.Create(), 1);
var exceedResult = await sender.SendAsync<TimingOrderConsumeRequest, ConsumeResult>(exceedRequest);
Console.WriteLine($"完成后消耗：{exceedResult}");