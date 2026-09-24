using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Orders;

[TestClass]
public sealed class OrderTests
{
    [TestMethod]
    public void Create_Success()
    {
        TimingOrder.CreateNow(CustomertId.Create(), EmployeeId.Create(), Money.CNY(3000), 30);
    }
}
