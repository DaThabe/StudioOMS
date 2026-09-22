using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Orders.TimingOrderTests;


[TestClass]
public sealed class ConsumeTests
{
    private const decimal _totalDays = 10;
    private readonly DateTimeOffset _orderCreateAt = DateTimeOffset.UtcNow;

    private TimingOrder _order = null!;
    private Employee _adminEmployee = null!;


    [TestInitialize]
    public void Setup()
    {
        _order = TimingOrder.Create(CustomertId.Create(), EmployeeId.Create(), _totalDays, _orderCreateAt);
        _adminEmployee = Employee.Create(EmployeeName.From("管理员"), [EmployeeRole.Admin]);
    }


    [TestMethod(DisplayName = "已分配员工在服务中可以成功消耗")]
    public void Consume_AssignedEmployeeInServicing_Succeeds()
    {
        var employeeId = EmployeeId.Create();
        _order.AssignEmployees(employeeId);
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _orderCreateAt);

        _order.Consume(employeeId, 1, _orderCreateAt.AddDays(1));

        Assert.AreEqual(1m, _order.UsedDays);
    }

    [TestMethod(DisplayName = "未标记服务中无法消耗")]
    public void Consume_NotServicing_Throws()
    {
        var employeeId = EmployeeId.Create();
        _order.AssignEmployees(employeeId);

        Assert.Throws<OrderStateOperationException>(() =>
            _order.Consume(employeeId, 1, _orderCreateAt.AddDays(1)));
    }

    [TestMethod(DisplayName = "未分配的员工无法消耗")]
    public void Consume_NotAssigned_Throws()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _orderCreateAt);

        Assert.Throws<OrderNotAssignedEmployeeException>(() =>
            _order.Consume(EmployeeId.Create(), 1, _orderCreateAt.AddDays(1)));
    }
}