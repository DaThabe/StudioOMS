using StudioOMS;
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
        _order = TimingOrder.Create(OrderId.Create(), ClientId.Create(), EmployeeId.Create(), _totalDays);
    }


    [TestMethod(DisplayName = "已分配员工在服务中可以成功消耗")]
    public void Success()
    {
        var employeeId = EmployeeId.Create();
        _order.AssignEmployees(employeeId);
        _order.MarkServicingNow();

        var consume = TimingConsume.CreateNow(ConsumeId.Create(), employeeId, 1);
        var result = _order.Consume(consume);

        Assert.IsInstanceOfType<TimingOrderConsumeResult.SuccessResult>(result);
        Assert.AreEqual(1m, _order.UsedDays);
    }


    [TestMethod(DisplayName = "未标记服务中无法消耗")]
    public void NotMarkServicing()
    {
        var consume = TimingConsume.CreateNow(ConsumeId.Create(), EmployeeId.Create(), 1);
        var result = _order.Consume(consume);

        Assert.IsInstanceOfType<TimingOrderConsumeResult.StateNotAllowedResultResult>(result);
    }

    [TestMethod(DisplayName = "未分配的员工无法消耗")]
    public void NotAssigned()
    {
        var markResult = _order.MarkServicingNow();
        Assert.IsInstanceOfType<OrderStateChangeResult.SuccessResult>(markResult);

        var consume = TimingConsume.CreateNow(ConsumeId.Create(), EmployeeId.Create(), 1);
        var result = _order.Consume(consume);

        Assert.IsInstanceOfType<TimingOrderConsumeResult.NotAssignedResult>(result);
    }
}