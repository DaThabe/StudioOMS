using StudioOMS;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOM.Orders.TimingOrderTests;


[TestClass]
public sealed class StateTests
{
    private const decimal _totalDays = 10;
    private TimingOrder _order = null!;


    [TestInitialize]
    public void Setup()
    {
        _order = TimingOrder.Create(OrderId.Create(), ClientId.Create(), EmployeeId.Create(), _totalDays);
    }


    [TestMethod(DisplayName = "待派发订单可以开始服务")]
    public void MarkServicing_FromWaiting_Succeeds()
    {
        var result = _order.MarkServicing(new DateTime(2026, 1, 1));
        Assert.IsInstanceOfType<StateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Servicing, _order.State);
    }

    [TestMethod(DisplayName = "服务中订单可以暂停")]
    public void MarkPaused_FromServicing_Succeeds()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        var result = _order.MarkPaused(new DateTime(2026, 1, 2));

        Assert.IsInstanceOfType<StateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Paused, _order.State);
    }

    [TestMethod(DisplayName = "暂停中订单可以恢复服务")]
    public void MarkServicing_FromPaused_Succeeds()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.MarkPaused(new DateTime(2026, 1, 2));
        var result = _order.MarkServicing(new DateTime(2026, 1, 3));

        Assert.IsInstanceOfType<StateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Servicing, _order.State);
    }


    [TestMethod(DisplayName = "已处于服务中的订单不能再次开始服务")]
    public void MarkServicing_WhenAlreadyServicing_ReturnsInvalidTransition()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        var result = _order.MarkServicing(new DateTime(2026, 1, 2));

        Assert.IsNotInstanceOfType<StateChangeResult.SuccessResult>(result);
    }

    [TestMethod(DisplayName = "待派发订单不能直接暂停")]
    public void MarkPaused_FromWaiting_ReturnsInvalidTransition()
    {
        var result = _order.MarkPaused(new DateTime(2026, 1, 1));
        Assert.IsNotInstanceOfType<StateChangeResult.SuccessResult>(result);
    }

    [TestMethod(DisplayName = "已完成的订单不能转为服务中")]
    public void MarkServicing_WhenCompleted_ReturnsInvalidTransition()
    {
        var employeeId = EmployeeId.Create();
        _order.AssignEmployees(employeeId);
        _order.MarkServicing(new DateTime(2026, 1, 1));

        var consume = TimingConsume.Create(
            ConsumeId.Create(), employeeId, _totalDays, new DateTime(2026, 1, 2));

        var result = _order.Consume(consume);

        Assert.IsInstanceOfType<ConsumeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Completed, _order.State);
    }
}