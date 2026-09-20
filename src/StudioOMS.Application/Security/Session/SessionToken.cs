using System.Diagnostics.CodeAnalysis;

namespace StudioOMS.Security.Session;


public sealed record class SessionToken
{
    private readonly Guid _value;
    public SessionToken(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static SessionToken Create() => new(Guid.CreateVersion7());
    public static SessionToken Parse(string guid) => new(Guid.Parse(guid));
    public static bool TryParse(string guid, [NotNullWhen(true)] out SessionToken? token)
    {
        if (!Guid.TryParse(guid, out var result))
        {
            token = default;
            return false;
        }

        token = new SessionToken(result);
        return true;
    }
}