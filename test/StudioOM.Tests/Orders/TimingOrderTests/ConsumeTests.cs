using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOM.Orders.TimingOrderTests;


[TestClass]
public sealed class ConsumeTests
{
    private const decimal _totalDays = 10;
    private TimingOrder _order = null!;


    [TestInitialize]
    public void Setup()
    {
        _order = TimingOrder.CreateNow(CustomertId.Create(), EmployeeId.Create(), _totalDays);
    }


    [TestMethod(DisplayName = "已分配员工在服务中可以成功消耗")]
    public void Success()
    {
        var employeeId = EmployeeId.Create();
        _order.AssignEmployees(employeeId);
        _order.MarkServicingNow();

        var consume = TimingOrderConsume.CreateNow(ConsumeId.Create(), employeeId, 1);
        _order.Consume(consume);

        Assert.AreEqual(1m, _order.UsedDays);
    }


    [TestMethod(DisplayName = "未标记服务中无法消耗")]
    public void NotMarkServicing()
    {
        var consume = TimingOrderConsume.CreateNow(ConsumeId.Create(), EmployeeId.Create(), 1);
        _order.Consume(consume);
    }

    [TestMethod(DisplayName = "未分配的员工无法消耗")]
    public void NotAssigned()
    {
        _order.MarkServicingNow();

        var consume = TimingOrderConsume.CreateNow(ConsumeId.Create(), EmployeeId.Create(), 1);
        _order.Consume(consume);
    }
}