using System.Diagnostics.CodeAnalysis;

namespace StudioOMS.Exceptions;

/// <summary>
/// 未认证异常
/// </summary>
public sealed class NotAuthenticatedException : Exception
{
    public NotAuthenticatedException() : base("未认证") { }
    public NotAuthenticatedException(string? message) : base(message) { }
    public NotAuthenticatedException(string? message, Exception? innerException) : base(message, innerException) { }


    [DoesNotReturn]
    public static void ThrowIf(bool condition)
    {
        if (condition)
            throw new NotAuthenticatedException();
    }
    [DoesNotReturn]
    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new NotAuthenticatedException(message);
    }
}
