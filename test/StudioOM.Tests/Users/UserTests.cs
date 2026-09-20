using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOM.Users;


[TestClass]
public class UserTests
{
    [TestMethod]
    public void TestMethod1()
    {
        var user1 = User.Create("123456", EmployeeId.Create());
        var user2 = User.Create("123456", EmployeeId.Create());

        Assert.IsFalse(user1 == user2);
    }
}
