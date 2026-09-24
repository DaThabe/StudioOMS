namespace StudioOMS.Users;


[TestClass]
public class PasswordTests
{
    [TestMethod(DisplayName = "密码空有空或空白字符")]
    public void From_HasNullOrEmptyChar_ThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            Password.From(" \t\n\r  "));
    }

    [TestMethod(DisplayName = "密码太短")]
    public void From_LengthIsShort_ThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            Password.From("123456"));
    }

    [TestMethod(DisplayName = "密码太长")]
    public void From_LengthIsLong_ThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            Password.From("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLINOPQRSTUVWXYZ0123456789!@#$%^&*()_+{}:~`"));
    }
}