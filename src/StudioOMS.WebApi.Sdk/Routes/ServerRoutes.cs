using StudioOMS.Orders;

namespace StudioOMS.Routes;


internal sealed class ServerRoutes(Uri baseUrl)
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

internal sealed class MeRoutes(Uri baseUrl)
{
    public string Info { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "me")
        .ToString();

    public string Password { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "me", "password")
        .ToString();
}


internal sealed class UserRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "users")
        .ToString();
}

internal sealed class EmployeeRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "employees")
        .ToString();
}

internal sealed class CustomerRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths("api", "customers")
        .ToString();
}

internal sealed class OrderRoutes(Uri baseUrl)
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