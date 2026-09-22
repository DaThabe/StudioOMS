namespace StudioOMS.Employees;


[TestClass]
public class EmployeeTests
{
    [TestMethod]
    public void TestMethod1()
    {
        EmployeeId.From(Guid.NewGuid());

        var id = EmployeeId.Create();
        var name = EmployeeName.From("User");

        var ex = Assert.Throws<EmployeeMustHaveRoleException>(() =>
            Employee.Create(id, name, []));

        Assert.AreEqual(id, ex.EmployeeId);
    }
}
