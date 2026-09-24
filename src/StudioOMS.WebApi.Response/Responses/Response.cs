namespace StudioOMS.Responses;


public readonly record struct Response<T>
     where T : notnull
{
    public required bool IsSuccess { get; init; }
    public string? Code { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }


    public T GetRequiredData()
    {
        if (Data is null) throw new InvalidOperationException("响应数据为空");
        return Data;
    }
}


public static class Response
{
    public static Response<NullResponseData> SuccessNotData { get; } = Success(NullResponseData.Null);

    public static Response<T> Success<T>(T data)
        where T : notnull
    {
        return new()
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static Response<NullResponseData> Error(string code, string? message = null)
    {
        return new()
        {
            IsSuccess = false,
            Code = code,
            Message = message
        };
    }
}