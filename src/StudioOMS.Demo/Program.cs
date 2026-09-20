using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Users;

var app = Host.CreateDefaultBuilder(args)
    .ConfigureServices(x =>
    {
        x.AddInfrastructure(x => x.UseSqlite("Data Source=studio_oms.db"));
        x.AddStudioOMSHandlers();
    })
    .Build();

await app.StartAsync();
var sender = app.Services.GetRequiredService<ISender>();


// 创建设计师
var designrEmployeeCreateRequest = new EmployeeCreateRequest()
{
    Id = EmployeeId.Create(),
    Name = "设计师"
};
await sender.SendAsync(designrEmployeeCreateRequest);

// 创建销售
var salespersonEmployeeCreateRequest = new EmployeeCreateRequest()
{
    Id = EmployeeId.Create(),
    Name = "销售"
};
await sender.SendAsync(salespersonEmployeeCreateRequest);

// 创建账号
var userCreateRequest = new UserCreateRequest()
{
    Id = UserId.Create(),
    Password = "123456789",
    Name = "设计师账号",
    EmployeeId = designrEmployeeCreateRequest.Id
};
await sender.SendAsync(userCreateRequest);

// 创建客户
var clientCreateRequest = new ClientCreateRequest()
{
    Id = ClientId.Create(),
    Name = "测试客户名称"
};
await sender.SendAsync(clientCreateRequest);

// 创建订单
var timingOrderCreateRequest = new TimingOrderCreateRequest()
{
    Id = OrderId.Create(),
    ClientId = clientCreateRequest.Id,
    SalespersonId = salespersonEmployeeCreateRequest.Id,
    CreateAt = DateTimeOffset.Now,
    TotalDays = 30,
    Title = "30天包月设计服务"
};
await sender.SendAsync(timingOrderCreateRequest);

// 分配设计师
var orderAssignEmployeeRequest = new OrderAssignEmployeeRequest()
{
    OrderId = timingOrderCreateRequest.Id,
    EmployeeId = designrEmployeeCreateRequest.Id
};
await sender.SendAsync(orderAssignEmployeeRequest);

// 订单开始服务
var orderMarkServicingRequest = new OrderMarkServicingRequest()
{
    OrderId = timingOrderCreateRequest.Id
};
await sender.SendAsync(orderMarkServicingRequest);

// 开始消耗
var timingOrderConsumeRequest = new TimingOrderConsumeRequest()
{
    OrderId = timingOrderCreateRequest.Id,
    ConsuemDays = 1,
    EmployeeId = designrEmployeeCreateRequest.Id,
};
await sender.SendAsync(timingOrderConsumeRequest);


await app.StopAsync();