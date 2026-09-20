using System.Diagnostics.CodeAnalysis;

namespace StudioOMS.Clients;


public readonly record struct ClientId : IId<ClientId>
{
    public static ClientId Empty => default;

    private readonly Guid _value;
    public ClientId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static ClientId Create() => new(Guid.CreateVersion7());
    public static ClientId Parse(string guid) => new(Guid.Parse(guid));
    public static bool TryParse(string str, [NotNullWhen(true)] out ClientId result)
    {
        if (!Guid.TryParse(str, out var guid))
        {
            result = default;
            return false;
        }

        result = new ClientId(guid);
        return true;
    }
}


public interface IId<TSelf>
{
    abstract static TSelf Create();
    abstract static TSelf Parse(string str);
    abstract static bool TryParse(string str, [NotNullWhen(true)] out TSelf result);
}