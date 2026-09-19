namespace StudioOMS;


public readonly record struct ClientId
{
    public static ClientId Empty => default;

    private readonly Guid _value;
    private ClientId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static ClientId Create() => new(Guid.CreateVersion7());
}