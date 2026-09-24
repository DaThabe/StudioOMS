namespace StudioOMS.Exceptions;


/// <summary>
/// 应用层异常基类
/// </summary>
public class AppException : Exception
{
    public AppException()
    {
    }

    public AppException(string? message) : base(message)
    {
    }

    public AppException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}