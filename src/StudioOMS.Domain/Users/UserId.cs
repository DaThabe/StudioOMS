namespace StudioOMS.Users;


public readonly record struct UserId
{
    public static UserId Empty => default;

    private readonly Guid _value;
    public UserId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static UserId Create() => new(Guid.CreateVersion7());
    public static UserId Parse(string guid) => new(Guid.Parse(guid));
}