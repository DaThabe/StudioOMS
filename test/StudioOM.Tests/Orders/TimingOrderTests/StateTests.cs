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


    #region --服务中--

    [TestMethod(DisplayName = "待派发订单可以开始服务")]
    public void MarkServicing_FromWaiting_Succeeds()
    {
        var result = _order.MarkServicing(new DateTime(2026, 1, 1));
        Assert.IsInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Servicing, _order.State);
    }

    [TestMethod(DisplayName = "暂停中订单可以恢复服务")]
    public void MarkServicing_FromPaused_Succeeds()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.MarkPaused(new DateTime(2026, 1, 2));
        var result = _order.MarkServicing(new DateTime(2026, 1, 3));

        Assert.IsInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Servicing, _order.State);
    }

    [TestMethod(DisplayName = "已处于服务中的订单不能再次开始服务")]
    public void MarkServicing_WhenAlreadyServicing_ReturnsInvalidTransition()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        var result = _order.MarkServicing(new DateTime(2026, 1, 2));

        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
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

    
    [TestMethod(DisplayName = "已终止的订单不能转为服务中")]
    public void MarkServicing_WhenTerminated_ReturnsNotAsExpected()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.MarkTerminated(new DateTime(2026, 1, 2));

        var result = _order.MarkServicing(new DateTime(2026, 1, 3));
        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
    }

    #endregion

    #region --暂停--

    [TestMethod(DisplayName = "服务中订单可以暂停")]
    public void MarkPaused_FromServicing_Succeeds()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        var result = _order.MarkPaused(new DateTime(2026, 1, 2));

        Assert.IsInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Paused, _order.State);
    }

    [TestMethod(DisplayName = "待派发订单不能直接暂停")]
    public void MarkPaused_FromWaiting_ReturnsInvalidTransition()
    {
        var result = _order.MarkPaused(new DateTime(2026, 1, 1));
        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
    }

    [TestMethod(DisplayName = "已终止的订单不能暂停")]
    public void MarkPaused_WhenTerminated_ReturnsNotAsExpected()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.MarkTerminated(new DateTime(2026, 1, 2));

        var result = _order.MarkPaused(new DateTime(2026, 1, 3));
        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
    }

    #endregion

    #region --取消--

    [TestMethod(DisplayName = "待派发的订单可以取消")]
    public void MarkCancelled_FromWaiting_Succeeds()
    {
        var result = _order.MarkCancelled(new DateTime(2026, 1, 1));

        Assert.IsInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Cancelled, _order.State);
    }

    [TestMethod(DisplayName = "服务中的订单不能取消")]
    public void MarkCancelled_FromServicing_ReturnsNotAsExpected()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));

        var result = _order.MarkCancelled(new DateTime(2026, 1, 2));

        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
    }

    [TestMethod(DisplayName = "已取消的订单不能转为服务中")]
    public void MarkServicing_WhenCancelled_ReturnsNotAsExpected()
    {
        _order.MarkCancelled(new DateTime(2026, 1, 1));

        var result = _order.MarkServicing(new DateTime(2026, 1, 2));

        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
    }

    #endregion

    #region --终止--

    [TestMethod(DisplayName = "暂停中的订单可以终止")]
    public void MarkTerminated_FromPaused_Succeeds()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.MarkPaused(new DateTime(2026, 1, 2));
        var result = _order.MarkTerminated(new DateTime(2026, 1, 3));

        Assert.IsInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Terminated, _order.State);
    }

    [TestMethod(DisplayName = "服务中的订单可以终止")]
    public void MarkTerminated_FromServicing_Succeeds()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        var result = _order.MarkTerminated(new DateTime(2026, 1, 2));

        Assert.IsInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
        Assert.AreEqual(OrderState.Terminated, _order.State);
    }


    [TestMethod(DisplayName = "待派发的订单不能直接终止")]
    public void MarkTerminated_FromWaiting_ReturnsNotAsExpected()
    {
        var result = _order.MarkTerminated(new DateTime(2026, 1, 1));
        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
    }

    [TestMethod(DisplayName = "已终止的订单不能再次终止")]
    public void MarkTerminated_WhenAlreadyTerminated_ReturnsNotAsExpected()
    {
        _order.MarkServicing(new DateTime(2026, 1, 1));
        _order.MarkTerminated(new DateTime(2026, 1, 2));

        var result = _order.MarkTerminated(new DateTime(2026, 1, 3));
        Assert.IsNotInstanceOfType<OrderStateChangeResult.SuccessResult>(result);
    }

    #endregion
}