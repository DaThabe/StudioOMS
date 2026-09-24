using StudioOMS.Exceptions;
using StudioOMS.Responses;
using StudioOMS.Serializer;

namespace StudioOMS.Middlewares;


internal sealed class ExceptionConverterMiddleware(
    RequestDelegate next,
    ILogger<ExceptionConverterMiddleware> logger,
    IExceptionConverter exceptionConverter
    )
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "领域异常: {Path}", context.Request.Path);
            await WriteErrorToResponseAsync(context, ex);
        }
        catch (AppException ex)
        {
            logger.LogWarning(ex, "应用异常: {Path}", context.Request.Path);
            await WriteErrorToResponseAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "参数异常: {Path}", context.Request.Path);
            await WriteErrorToResponseAsync(context, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "未处理异常: {Path}", context.Request.Path);
            await WriteErrorToResponseAsync(context, ex);
        }
    }

    private async Task WriteErrorToResponseAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            logger.LogError(exception, "响应已开始，无法写入错误响应");
            return;
        }

        try
        {
            var errorResponse = exceptionConverter.Convert(exception);

            context.Response.StatusCode = (int)errorResponse.StatusCode;
            await context.Response.WriteAsJsonAsync(errorResponse.ToResponse(), ResponseJsonSerializerContext.Default.ResponseNullResponseData);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "写入错误响应失败");

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal Server Error");
            }
        }
    }
}