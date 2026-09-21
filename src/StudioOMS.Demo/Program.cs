using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Requests;
using StudioOMS.Requests.Customers;
using StudioOMS.Requests.Employees;
using StudioOMS.Requests.Login;
using StudioOMS.Requests.Orders;
using StudioOMS.Requests.Orders.Timing;
using StudioOMS.Security;
using StudioOMS.Security.Session;
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
var sessionService = app.Services.GetRequiredService<ISessionService>();
var curentSession = app.Services.GetRequiredService<ICurrentSession>();
var employeeRepository = app.Services.GetRequiredService<IEmployeeRepository>();
var userRepository = app.Services.GetRequiredService<IUserRepository>();
var passwordHasher = app.Services.GetRequiredService<IPasswordHasher>();

const string adminUsername = "admin";
const string adminPassword = "123456789";

// 初始化管理员用户
if (await userRepository.FindByUsername(adminUsername) is null)
{
    // 员工
    var employee = Employee.Create([EmployeeRole.Admin]);
    employee.Rename("管理员");
    await employeeRepository.SaveAsync(employee);

    // 用户
    var hashedPwd = await passwordHasher.HashAsync(adminPassword);
    var user = User.Create(adminUsername, hashedPwd, employee.Id);
    await userRepository.SaveAsync(user);
}

// 登录
var adminUserLoginRequest = new LoginRequest()
{
    Username = adminUsername,
    Password = adminPassword,
};
// 初始化当前会话
curentSession.Token = await sender.SendAsync<LoginRequest, SessionToken>(adminUserLoginRequest);
curentSession.Info = await sessionService.FindAsync(curentSession.Token) ?? throw new InvalidOperationException("会话信息不存在");


// 创建设计师
var designrEmployeeCreateRequest = new EmployeeCreateRequest()
{
    Name = "设计师",
    Roles = [EmployeeRole.Designer]
};
var designerId = await sender.SendAsync<EmployeeCreateRequest, EmployeeId>(designrEmployeeCreateRequest);

// 创建销售
var salespersonEmployeeCreateRequest = new EmployeeCreateRequest()
{
    Name = "销售",
    Roles = [EmployeeRole.Sales]
};
var salespersonId = await sender.SendAsync<EmployeeCreateRequest, EmployeeId>(salespersonEmployeeCreateRequest);

// 创建客户
var customerCreateRequest = new CustomerCreateRequest()
{
    Name = "测试客户名称"
};
var customerId = await sender.SendAsync<CustomerCreateRequest, CustomertId>(customerCreateRequest);

// 创建订单
var timingOrderCreateRequest = new TimingOrderCreateRequest()
{
    CustomerId = customerId,
    SalespersonId = salespersonId,
    TotalDays = 30,
    Title = "30天包月设计服务"
};
var orderId = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(timingOrderCreateRequest);

// 分配设计师
var orderAssignEmployeeRequest = new OrderAssignEmployeeRequest()
{
    Id = orderId,
    EmployeeId = designerId,
};
await sender.SendAsync(orderAssignEmployeeRequest);

// 订单开始服务
var orderMarkServicingRequest = new OrderMarkServicingRequest()
{
    OrderId = orderId
};
await sender.SendAsync(orderMarkServicingRequest);

// 开始消耗
var timingOrderConsumeRequest = new TimingOrderConsumeRequest()
{
    OrderId = orderId,
    ConsuemDays = 1,
    EmployeeId = designerId,
};
await sender.SendAsync(timingOrderConsumeRequest);


await app.StopAsync();