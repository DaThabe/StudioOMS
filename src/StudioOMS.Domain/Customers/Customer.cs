namespace StudioOMS.Customers;


public sealed class Customer : Entity<CustomertId>
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



    internal Customer() { }
    public static Customer Create(CustomertId customerId)
    {
        if (customerId == CustomertId.Empty)
            throw new ArgumentException("客户 Id 不可为空", nameof(customerId));


        return new()
        {
            Id = customerId
        };
    }
    public static Customer Create() =>
        Create(CustomertId.Create());
}