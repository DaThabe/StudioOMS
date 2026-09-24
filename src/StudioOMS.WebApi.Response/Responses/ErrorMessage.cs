using System.Net;

namespace StudioOMS.WebApi.Responses;


public readonly record struct ErrorMessage
{
    public required HttpStatusCode StatusCode { get; init; }
    public required string ErrorCode { get; init; }
    public required string Message { get; init; }


    public Response<NullResponseData> ToResponse() =>
        Response.Error(ErrorCode, Message);



    public static ErrorMessage Create(HttpStatusCode httpStatusCode, string errorCode, string message)
    {
        return new() { StatusCode = httpStatusCode, ErrorCode = errorCode, Message = message };
    }

    public static ErrorMessage Unauthorized(string errorCode, string message) =>
        Create(HttpStatusCode.Unauthorized, errorCode, message);
    public static ErrorMessage Forbidden(string errorCode, string message) =>
        Create(HttpStatusCode.Forbidden, errorCode, message);


    public static ErrorMessage BadRequest(string errorCode, string message) =>
        Create(HttpStatusCode.BadRequest, errorCode, message);
    public static ErrorMessage InternalServerError(string errorCode, string message) =>
        Create(HttpStatusCode.InternalServerError, errorCode, message);
}