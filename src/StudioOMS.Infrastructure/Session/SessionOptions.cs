namespace StudioOMS.Security;


public sealed class SessionOptions
{
    public const string SelectionName = "Session";
    public const string SelectionPath = $"StudioOMS:{SelectionName}";


    public required TimeSpan Lifetime { get; set; } = TimeSpan.FromDays(30);
}
