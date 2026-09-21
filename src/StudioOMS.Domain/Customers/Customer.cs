namespace StudioOMS.Customers;


public sealed class Customer : Entity<CustomertId>
{
    public CustomerName Name { get; private set; }


    public void Rename(CustomerName value)
    {
        if (value.IsExactlySameAs(Name)) return;

        // 更新
        Name = value;
    }



    private Customer(CustomerName name) => Name = name;
    public static Customer Create(CustomertId customerId, CustomerName name) =>
        new(name) { Id = customerId };
    public static Customer Create(CustomerName name) =>
        Create(CustomertId.Create(), name);
}