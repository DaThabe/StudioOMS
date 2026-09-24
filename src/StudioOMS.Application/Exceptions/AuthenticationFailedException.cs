namespace StudioOMS.Exceptions;

/// <summary>
/// 认证失败
/// </summary>
#pragma warning disable RCS1194 // Implement exception constructors
public sealed class AuthenticationFailedException() : AppException("用户名或密码错误");
#pragma warning restore RCS1194 // Implement exception constructors