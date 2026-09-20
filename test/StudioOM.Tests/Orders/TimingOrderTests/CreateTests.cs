using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOM.Orders.TimingOrderTests;


[TestClass]
public sealed class CreateTests
{
    [TestMethod(DisplayName = "订单Id为空, 抛出(ArgumentException)")]
    public void OrderIdEmpty_ThrowArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            TimingOrder.CreateNow(OrderId.Empty, ClientId.Create(), EmployeeId.Create(), 10));

        Assert.AreEqual("orderId", ex.ParamName);
    }

    [TestMethod(DisplayName = "客户Id为空, 抛出(ArgumentException)")]
    public void ClientIdIdEmpty_ThrowArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            TimingOrder.CreateNow(OrderId.Create(), ClientId.Empty, EmployeeId.Create(), 10));

        Assert.AreEqual("clientId", ex.ParamName);
    }

    [TestMethod(DisplayName = "销售员工Id为空, 抛出(ArgumentException)")]
    public void SalespersonIdIdIdEmpty_ThrowArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            TimingOrder.CreateNow(OrderId.Create(), ClientId.Create(), EmployeeId.Empty, 10));

        Assert.AreEqual("salespersonId", ex.ParamName);
    }

    [TestMethod(DisplayName = "总天数小于等于0, 抛出(ArgumentOutOfRangeException)")]
    public void TotalDaysNotPositive_ThrowArgumentOutOfRangeException()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            TimingOrder.CreateNow(OrderId.Create(), ClientId.Create(), EmployeeId.Create(), 0));

        Assert.AreEqual("totalDays", ex.ParamName);
    }
}
