namespace StudioOMS.Exceptions;

/// <summary>
/// 未认证异常
/// </summary>
public sealed class NotAuthenticatedException : Exception
{
    public NotAuthenticatedException() : base("未认证") { }
    public NotAuthenticatedException(string? message) : base(message) { }
    public NotAuthenticatedException(string? message, Exception? innerException) : base(message, innerException) { }


    public static void ThrowIf(bool condition)
    {
        if (condition)
            throw new NotAuthenticatedException();
    }
    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new NotAuthenticatedException(message);
    }
}
