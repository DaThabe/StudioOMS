using Microsoft.Extensions.DependencyInjection;
using StudioOMS;
using StudioOMS.Demo;

// Services
var descriptors = new ServiceCollection();
descriptors.AddMessagingSender();
descriptors.AddStudioOMSHandlers();
descriptors.AddStudioOMSRepository();
descriptors.AddStudioOMSClient();

// Client
var services = descriptors.BuildServiceProvider();
var client = services.GetRequiredService<StudioOMSClient>();

// 创建订单
var totalDays = 10;
var order = await client.CreateTimingOrderAsync(ClientId.Create(), EmployeeId.Create(), 10);

// 分配设计师
var designerId = EmployeeId.Create();
await order.AssignEmployeeAsync(designerId);

// 订单开始服务
await order.MarkServicingAsync();

// 开始消耗
for (int i = 0; i < totalDays; i++)
{
    var consumeResult = await order.ConsumeAsync(designerId, 1);
    Console.WriteLine($"划扣结果: {consumeResult}");
}

// 超额消耗
var exceedResult = await order.ConsumeAsync(designerId, 1);
Console.WriteLine($"完成后消耗：{exceedResult}");