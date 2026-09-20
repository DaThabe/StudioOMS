namespace StudioOMS.Clients;


public sealed class Client : Entity<ClientId>
{
    public string Name { get; private set; } = "未命名客户";



    public void Rename(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var trimmed = value.Trim();

        // 相同
        if (string.Equals(trimmed, Name, StringComparison.OrdinalIgnoreCase))
            return;

        Name = trimmed;
    }



    internal Client() { }
    public static Client Create(ClientId clientId)
    {
        if (clientId == ClientId.Empty)
            throw new ArgumentException("客户 Id 不可为空", nameof(clientId));


        return new()
        {
            Id = clientId
        };
    }
    public static Client Create() =>
        Create(ClientId.Create());
}