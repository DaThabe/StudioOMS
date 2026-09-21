using StudioOMS.Orders;

namespace StudioOMS.Routes;


public sealed class ServerRoutes(Uri baseUrl)
{
    public string Login { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "login")
        .ToString();


    public MeRoutes Me { get; init; } = new(baseUrl);
    public UserRoutes User { get; init; } = new(baseUrl);
    public EmployeeRoutes Employee { get; init; } = new(baseUrl);
    public CustomerRoutes Customer { get; init; } = new(baseUrl);
    public OrderRoutes Order { get; init; } = new(baseUrl);
}

public sealed class MeRoutes(Uri baseUrl)
{
    public string Password { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "me", "password")
        .ToString();
}


public sealed class UserRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "users")
        .ToString();
}

public sealed class EmployeeRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "employees")
        .ToString();
}

public sealed class CustomerRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "customers")
        .ToString();
}

public sealed class OrderRoutes(Uri baseUrl)
{
    public string List(OrderListDto dto) => new UrlBuilder(baseUrl)
        .AddPaths("api", "orders")
        .AddQuery(nameof(dto.Take), dto.Take?.ToString())
        .AddQuery(nameof(dto.Skip), dto.Skip?.ToString())
        .AddQuery(nameof(dto.Types), dto.Types)
        .ToString();


    public string CreateTiming { get; } = new UrlBuilder(baseUrl)
       .AddPaths("api", "orders", "timing")
       .ToString();

    public string Assign(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths("api", "orders", orderId.ToString(), "assign")
        .ToString();

    public string ConsumeTiming(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths("api", "orders", orderId.ToString(), "consume-timing")
        .ToString();
}