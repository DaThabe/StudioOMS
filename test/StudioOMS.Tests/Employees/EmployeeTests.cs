namespace StudioOMS.Employees;


[TestClass]
public class EmployeeTests
{
    [TestMethod(DisplayName = "创建员工至少包含一个职位, 缺少则抛出(EmployeeMustHaveRoleException)")]
    public void Create_MissingRole_ThrowEmployeeMustHaveRoleException()
    {
        EmployeeId.From(Guid.NewGuid());

        var id = EmployeeId.Create();
        var name = EmployeeName.From("User");

        var ex = Assert.Throws<EmployeeMustHaveRoleException>(() =>
            Employee.Create(id, name, []));

        Assert.AreEqual(id, ex.EmployeeId);
    }
}
