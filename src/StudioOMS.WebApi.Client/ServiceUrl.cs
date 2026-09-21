namespace StudioOMS;


public sealed record class ServiceUrl
{
    public required Uri BaseUrl { get; init; }
    public string UserLoginPath => $"{BaseUrl}/users/login";
}