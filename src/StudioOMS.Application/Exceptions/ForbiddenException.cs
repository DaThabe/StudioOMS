namespace StudioOMS.Exceptions;


/// <summary>
/// 权限不足异常
/// </summary>
public sealed class ForbiddenException : Exception
{
    public ForbiddenException() : base("权限不足") { }
    public ForbiddenException(string? message) : base(message) { }
    public ForbiddenException(string? message, Exception? innerException) : base(message, innerException) { }



    public static void ThrowIf(bool condition)
    {
        if (condition)
            throw new ForbiddenException();
    }
    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new ForbiddenException();
    }
}
