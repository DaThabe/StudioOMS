namespace StudioOMS.Security.Session;


public sealed record class SessionToken
{
    private readonly Guid _value;
    public SessionToken(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static SessionToken Create() => new(Guid.CreateVersion7());
    public static SessionToken Parse(string guid) => new(Guid.Parse(guid));
}