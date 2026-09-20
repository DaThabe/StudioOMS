namespace StudioOMS.Clients;


public readonly record struct ClientId
{
    public static ClientId Empty => default;

    private readonly Guid _value;
    public ClientId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static ClientId Create() => new(Guid.CreateVersion7());
    public static ClientId Parse(string guid) => new(Guid.Parse(guid));
}