namespace StudioOMS.Responses;


public sealed record class NullResponseData
{
    private NullResponseData() { }
    public static NullResponseData Null { get; } = new();
}