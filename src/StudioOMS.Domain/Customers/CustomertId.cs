using System.Diagnostics.CodeAnalysis;

namespace StudioOMS.Customers;


public readonly record struct CustomertId : IId<CustomertId>
{
    public static CustomertId Empty => default;

    private readonly Guid _value;
    public CustomertId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static CustomertId Create() => new(Guid.CreateVersion7());
    public static CustomertId Parse(string guid) => new(Guid.Parse(guid));
    public static bool TryParse(string str, [NotNullWhen(true)] out CustomertId result)
    {
        if (!Guid.TryParse(str, out var guid))
        {
            result = default;
            return false;
        }

        result = new CustomertId(guid);
        return true;
    }
}


public interface IId<TSelf>
{
    abstract static TSelf Create();
    abstract static TSelf Parse(string str);
    abstract static bool TryParse(string str, [NotNullWhen(true)] out TSelf result);
}