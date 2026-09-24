using StudioOMS.WebApi.Responses;

namespace StudioOMS.WebApi;


internal static class ResponseResults
{
    public static IResult Ok<T>(T data) where T : notnull =>
        Results.Ok(Response.Success(data));

    public static IResult Ok() =>
        Results.Ok(Response.SuccessNotData);
}
