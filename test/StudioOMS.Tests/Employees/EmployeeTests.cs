namespace StudioOMS.Employees;


[TestClass]
public class EmployeeTests
{
    [TestMethod]
    public void TestMethod1()
    {
        var id = EmployeeId.Create();

        var ex = Assert.Throws<EmployeeMustHaveRoleException>(() =>
            Employee.Create(id, EmployeeName.From("User"), []));

        Assert.AreEqual(id, ex.EmployeeId);
    }
}
