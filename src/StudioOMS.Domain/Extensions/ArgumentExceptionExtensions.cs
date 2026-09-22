using System.Runtime.CompilerServices;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace System;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

internal static class ArgumentExceptionExtensions
{
    extension(ArgumentException)
    {
        public static void ThrowIfNotDefined<T>(T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) 
            where T :struct, Enum
        {
            if (Enum.IsDefined(argument)) return;
            throw new ArgumentNullException(paramName);
        }
    }
}
