namespace StudioOMS.Exceptions;


/// <summary>
/// 未认证异常
/// </summary>
#pragma warning disable RCS1194 // Implement exception constructors
public sealed class NotAuthenticatedException() : AppException("未认证")
#pragma warning restore RCS1194 // Implement exception constructors
{
    public static void ThrowIf(bool condition)
    {
        if (condition)
            throw new NotAuthenticatedException();
    }
}
