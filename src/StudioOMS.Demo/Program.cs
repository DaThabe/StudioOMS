using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Requests;
using StudioOMS.Requests.Clients;
using StudioOMS.Requests.Employees;
using StudioOMS.Requests.Orders;
using StudioOMS.Requests.Orders.Timing;
using StudioOMS.Requests.Users;
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
var currentUser = app.Services.GetRequiredService<ICurrentUser>();
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
var adminUserLoginRequest = new UserLoginRequest()
{
    Username = adminUsername,
    Password = adminPassword,
};
var adminLoginToken = await sender.SendAsync<UserLoginRequest, SessionToken>(adminUserLoginRequest);
var adminSessionInfo = await sessionService.FindAsync(adminLoginToken) ?? throw new InvalidOperationException("会话信息不存在");
currentUser.EmployeeId = adminSessionInfo.EmployeeId;


// 创建设计师
var designrEmployeeCreateRequest = new EmployeeCreateRequest()
{
    Id = EmployeeId.Create(),
    Name = "设计师",
    Roles = [EmployeeRole.Designer]
};
await sender.SendAsync(designrEmployeeCreateRequest);

// 创建销售
var salespersonEmployeeCreateRequest = new EmployeeCreateRequest()
{
    Id = EmployeeId.Create(),
    Name = "销售",
    Roles = [EmployeeRole.Sales]
};
await sender.SendAsync(salespersonEmployeeCreateRequest);

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