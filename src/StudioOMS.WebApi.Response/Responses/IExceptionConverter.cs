using StudioOMS.Exceptions;
using StudioOMS.Orders;

namespace StudioOMS.WebApi.Responses;


public interface IExceptionConverter
{
    ErrorMessage Convert(Exception exception);
}

internal sealed class ExceptionConverter : IExceptionConverter
{
    public ErrorMessage Convert(Exception exception) => exception switch
    {
        // 未认证
        NotAuthenticatedException => ErrorMessage
            .Unauthorized(ErrorCodes.NotLogin, "未登录"),
        // 权限不足
        ForbiddenException => ErrorMessage
            .Forbidden("", "不允许操作"),


        // 订单类异常
        OrderConsumeExceedsLimitException => ErrorMessage
            .BadRequest(ErrorCodes.Order.ConsumeExceedsLimit, "订单划扣超过上限"),
        OrderConsumeTimestampInvalidException => ErrorMessage
            .BadRequest(ErrorCodes.Order.ConsumeTimeInvalid, "订单划扣时间异常"),



        // 领域或App异常
        DomainException or AppException => ErrorMessage.BadRequest("", "请求错误"),
        // 其他异常
        _ => ErrorMessage.InternalServerError("", "内部系统异常, 请稍后重试")
    };
}