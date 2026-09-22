using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.EfCore;


internal sealed class InitAdminHostedService(IServiceScopeFactory scopeFactory, ILogger<InitAdminHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        // 数据库存在员工
        if (await employeeRepository.AnyAsync(cancellationToken)) return;

        // 创建管理员

        // 员工
        var employee = Employee.Create(EmployeeName.From("管理员"), [EmployeeRole.Admin]);
        await employeeRepository.SaveAsync(employee, cancellationToken);

        // 用户
        var initPassword = Password.Create();
        var hashedPwd = await passwordHasher.HashAsync(initPassword, cancellationToken);

        var user = User.Create(Username.From("admin"), hashedPwd, employee.Id);
        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation("初始管理员用户已创建, Username: {Name}, Password={Pwd}", user.Username, initPassword.GetRawText());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}