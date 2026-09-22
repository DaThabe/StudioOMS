using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Orders.TimingOrderTests;


[TestClass]
public sealed class StateTests
{
    private const decimal _totalDays = 10;
    private readonly DateTimeOffset _time = DateTimeOffset.Now;

    private TimingOrder _order = null!;
    private Employee _adminEmployee = null!;


    [TestInitialize]
    public void Setup()
    {
        _order = TimingOrder.CreateNow(CustomertId.Create(), EmployeeId.Create(), _totalDays);
        _adminEmployee = Employee.Create(EmployeeName.From("管理员"), [EmployeeRole.Admin]);
    }


    #region --服务中--

    [TestMethod(DisplayName = "待派发订单可以开始服务")]
    public void MarkServicing_FromWaiting_Succeeds()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        Assert.AreEqual(OrderState.Servicing, _order.State);
    }

    [TestMethod(DisplayName = "暂停中订单可以恢复服务")]
    public void MarkServicing_FromPaused_Succeeds()
    {
        // 先到 Paused
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        OrderStatePolicy.MarkPaused(_order, _adminEmployee, _time.AddDays(1));

        // 再从 Paused 恢复
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time.AddDays(2));

        Assert.AreEqual(OrderState.Servicing, _order.State);
    }

    [TestMethod(DisplayName = "已处于服务中的订单不能再次开始服务")]
    public void MarkServicing_WhenAlreadyServicing_Throws()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);

        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time.AddDays(1)));
    }

    [TestMethod(DisplayName = "已完成的订单不能转为服务中")]
    public void MarkServicing_WhenCompleted_Throws()
    {
        var employeeId = EmployeeId.Create();
        _order.AssignEmployees(employeeId);
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);

        _order.Consume(employeeId, _totalDays, _time.AddDays(1));

        Assert.AreEqual(OrderState.Completed, _order.State);

        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time.AddDays(2)));
    }

    [TestMethod(DisplayName = "已终止的订单不能转为服务中")]
    public void MarkServicing_WhenTerminated_Throws()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        OrderStatePolicy.MarkTerminated(_order, _adminEmployee, _time.AddDays(1));

        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time.AddDays(2)));
    }

    #endregion

    #region --暂停--

    [TestMethod(DisplayName = "服务中订单可以暂停")]
    public void MarkPaused_FromServicing_Succeeds()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        OrderStatePolicy.MarkPaused(_order, _adminEmployee, _time.AddDays(1));

        Assert.AreEqual(OrderState.Paused, _order.State);
    }

    [TestMethod(DisplayName = "待派发订单不能直接暂停")]
    public void MarkPaused_FromWaiting_Throws()
    {
        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkPaused(_order, _adminEmployee, _time));
    }

    [TestMethod(DisplayName = "已终止的订单不能暂停")]
    public void MarkPaused_WhenTerminated_Throws()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        OrderStatePolicy.MarkTerminated(_order, _adminEmployee, _time.AddDays(1));

        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkPaused(_order, _adminEmployee, _time.AddDays(2)));
    }

    #endregion

    #region --取消--

    [TestMethod(DisplayName = "待派发的订单可以取消")]
    public void MarkCancelled_FromWaiting_Succeeds()
    {
        OrderStatePolicy.MarkCancelled(_order, _adminEmployee, _time);

        Assert.AreEqual(OrderState.Cancelled, _order.State);
    }

    [TestMethod(DisplayName = "服务中的订单不能取消")]
    public void MarkCancelled_FromServicing_Throws()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);

        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkCancelled(_order, _adminEmployee, _time.AddDays(1)));
    }

    [TestMethod(DisplayName = "已取消的订单不能转为服务中")]
    public void MarkServicing_WhenCancelled_Throws()
    {
        OrderStatePolicy.MarkCancelled(_order, _adminEmployee, _time);

        Assert.Throws<OrderStateTransitionNotAllowedException>(
            () => OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time.AddDays(1)));
    }

    #endregion

    #region --终止--

    [TestMethod(DisplayName = "暂停中的订单可以终止")]
    public void MarkTerminated_FromPaused_Succeeds()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        OrderStatePolicy.MarkPaused(_order, _adminEmployee, _time.AddDays(1));
        OrderStatePolicy.MarkTerminated(_order, _adminEmployee, _time.AddDays(2));

        Assert.AreEqual(OrderState.Terminated, _order.State);
    }

    [TestMethod(DisplayName = "服务中的订单可以终止")]
    public void MarkTerminated_FromServicing_Succeeds()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        OrderStatePolicy.MarkTerminated(_order, _adminEmployee, _time.AddDays(1));

        Assert.AreEqual(OrderState.Terminated, _order.State);
    }

    [TestMethod(DisplayName = "待派发的订单不能直接终止")]
    public void MarkTerminated_FromWaiting_Throws()
    {
        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkTerminated(_order, _adminEmployee, _time));
    }

    [TestMethod(DisplayName = "已终止的订单不能再次终止")]
    public void MarkTerminated_WhenAlreadyTerminated_Throws()
    {
        OrderStatePolicy.MarkServicing(_order, _adminEmployee, _time);
        OrderStatePolicy.MarkTerminated(_order, _adminEmployee, _time.AddDays(1));

        Assert.Throws<OrderStateTransitionNotAllowedException>(() =>
            OrderStatePolicy.MarkTerminated(_order, _adminEmployee, _time.AddDays(2)));
    }

    #endregion
}