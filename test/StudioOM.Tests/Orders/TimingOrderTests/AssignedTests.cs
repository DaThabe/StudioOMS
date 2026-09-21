using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOM.Orders.TimingOrderTests;


[TestClass]
public sealed class AssignedTests
{
    private const decimal _totalDays = 10;
    private TimingOrder _order = null!;


    [TestInitialize]
    public void Setup()
    {
        _order = TimingOrder.CreateNow(CustomertId.Create(), EmployeeId.Create(), _totalDays);
    }



    [TestMethod(DisplayName = "待派发订单可以派发员工")]
    public void AssignEmployee_FromWaiting_Succeeds()
    {
        var employeeId = EmployeeId.Create();

        _order.AssignEmployees(employeeId);

        Assert.Contains(employeeId, _order.AssignedEmployees);
    }

    [TestMethod(DisplayName = "服务中的订单可以加派员工")]
    public void AssignEmployee_FromServicing_Succeeds()
    {
        var first = EmployeeId.Create();
        var second = EmployeeId.Create();

        _order.AssignEmployees(first);
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.AssignEmployees(second);

        Assert.Contains(first, _order.AssignedEmployees);
        Assert.Contains(second, _order.AssignedEmployees);
    }

    [TestMethod(DisplayName = "已完成的订单不能派发员工")]
    public void AssignEmployee_WhenCompleted_Throws()
    {
        var employeeId = EmployeeId.Create();
        _order.AssignEmployees(employeeId);
        _order.MarkServicing(new DateTime(2026, 1, 1));

        var consume = TimingOrderConsume.Create(
            ConsumeId.Create(), employeeId, _totalDays, new DateTime(2026, 1, 2));
        _order.Consume(consume);  // 订单完成

        Assert.Throws<InvalidOperationException>(() =>
            _order.AssignEmployees(EmployeeId.Create()));
    }

    [TestMethod(DisplayName = "已终止的订单不能派发员工")]
    public void AssignEmployee_WhenTerminated_Throws()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.MarkTerminated(new DateTime(2026, 1, 2));

        Assert.Throws<InvalidOperationException>(() =>
            _order.AssignEmployees(EmployeeId.Create()));
    }

    [TestMethod(DisplayName = "已取消的订单不能派发员工")]
    public void AssignEmployee_WhenCancelled_Throws()
    {
        _order.MarkCancelled(new DateTime(2026, 1, 1));

        Assert.Throws<InvalidOperationException>(() =>
            _order.AssignEmployees(EmployeeId.Create()));
    }

    [TestMethod(DisplayName = "重复派发同一员工不产生重复记录")]
    public void AssignEmployee_SameEmployeeTwice_OnlyOneRecord()
    {
        var employeeId = EmployeeId.Create();

        _order.AssignEmployees(employeeId);
        _order.AssignEmployees(employeeId);

        Assert.HasCount(1, _order.AssignedEmployees);
    }
}
