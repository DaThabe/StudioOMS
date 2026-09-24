using StudioOMS.Responses;

namespace StudioOMS;


internal static class ResponseResults
{
    public static IResult Ok<T>(T data) =>
        Results.Ok(Response.Success(data));

    public static IResult Ok() =>
        Results.Ok(Response.SuccessNotData);
}
