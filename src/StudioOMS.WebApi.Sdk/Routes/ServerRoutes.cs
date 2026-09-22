using StudioOMS.Orders;

namespace StudioOMS.Routes;


internal sealed class ServerRoutes(Uri baseUrl)
{
    public string Login { get; } = new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.Login)
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
        .AddPaths(WebApiRoutes.MePassword)
        .ToString();

    public string Password { get; } = new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.MePassword)
        .ToString();
}


internal sealed class UserRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.Users)
        .ToString();
}

internal sealed class EmployeeRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.Employees)
        .ToString();
}

internal sealed class CustomerRoutes(Uri baseUrl)
{
    public string Create { get; } = new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.Customers)
        .ToString();
}

internal sealed class OrderRoutes(Uri baseUrl)
{
    public string List(OrderListDto dto) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.Orders)
        .AddQuery(nameof(dto.Take), dto.Take?.ToString())
        .AddQuery(nameof(dto.Skip), dto.Skip?.ToString())
        .AddQuery(nameof(dto.Types), dto.Types)
        .ToString();

    public string Assign(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.OrdersIdAssign.Replace("{id}", orderId.ToString()))
        .ToString();

    public string MarServicing(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.OrdersIdServicing.Replace("{id}", orderId.ToString()))
        .ToString();
    public string MarkPaused(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.OrdersIdPaused.Replace("{id}", orderId.ToString()))
        .ToString();
    public string MarkCancelle(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.OrdersIdCancelled.Replace("{id}", orderId.ToString()))
        .ToString();
    public string MarkTerminated(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.OrdersIdTerminated.Replace("{id}", orderId.ToString()))
        .ToString();


    public string CreateTiming { get; } = new UrlBuilder(baseUrl)
      .AddPaths(WebApiRoutes.OrdersTiming)
      .ToString();

    public string ConsumeTiming(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.OrdersIdTimingConsume.Replace("{id}", orderId.ToString()))
        .ToString();
}