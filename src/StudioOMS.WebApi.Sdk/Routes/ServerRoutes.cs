using StudioOMS.Orders;

namespace StudioOMS.WebApi.Routes;


internal sealed class ServerRoutes(Uri baseUrl)
{
    public string Login { get; } = new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.Login)
        .ToString();

    public MeRoutes Me { get; init; } = new(baseUrl);
    public UserRoutes User { get; init; } = new(baseUrl);
    public EmployeeRoutes Employee { get; init; } = new(baseUrl);
    public CustomerRoutes Customer { get; init; } = new(baseUrl);
    public OrderRoutes Order { get; init; } = new(baseUrl);
}

internal sealed class MeRoutes(Uri baseUrl)
{
    public string Me { get; } = new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.Me)
        .ToString();

    public string Password { get; } = new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.MePassword)
        .ToString();
}


internal sealed class UserRoutes(Uri baseUrl)
{
    public string Users { get; } = new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.Users)
        .ToString();
}

internal sealed class EmployeeRoutes(Uri baseUrl)
{
    public string Employees { get; } = new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.Employees)
        .ToString();
}

internal sealed class CustomerRoutes(Uri baseUrl)
{
    public string Customers { get; } = new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.Customers)
        .ToString();
}

internal sealed class OrderRoutes(Uri baseUrl)
{
    public string Orders(OrderListDto dto) => new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.Orders)
        .AddQuery(nameof(dto.Take), dto.Take?.ToString())
        .AddQuery(nameof(dto.Skip), dto.Skip?.ToString())
        .AddQuery(nameof(dto.Types), dto.Types)
        .ToString();

    public string Assign(Guid orderId) => new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.OrdersIdAssign.Replace("{id}", orderId.ToString()))
        .ToString();

    public string MarServicing(Guid orderId) => new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.OrdersIdServicing.Replace("{id}", orderId.ToString()))
        .ToString();
    public string MarkPaused(Guid orderId) => new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.OrdersIdPaused.Replace("{id}", orderId.ToString()))
        .ToString();
    public string MarkCancelle(Guid orderId) => new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.OrdersIdCancelled.Replace("{id}", orderId.ToString()))
        .ToString();
    public string MarkTerminated(Guid orderId) => new UrlBuilder(baseUrl)
        .AddPaths(WebApiRoutes.OrdersIdTerminated.Replace("{id}", orderId.ToString()))
        .ToString();



    public string CreateTiming { get; } = new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.OrdersTiming)
        .ToString();

    public string ConsumeTiming(Guid orderId) => new UrlBuilder(baseUrl)
        .AddSegment(WebApiRoutes.OrdersIdTimingConsume.Replace("{id}", orderId.ToString()))
        .ToString();
}