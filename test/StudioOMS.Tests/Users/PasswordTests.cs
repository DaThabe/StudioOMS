using StudioOMS.Users;

namespace StudioOMS.Users;


[TestClass]
public class PasswordTests
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.Throws<ArgumentException>(() =>
            Password.From("123456"));
    }
}
