namespace StudioOMS.WebApi.Endpoints;


internal static class EndpointExtensions
{
    public static void MapStudioOMS(this IEndpointRouteBuilder builder)
    {
        // POST 👉 api/login
        builder.MapPost(WebApiRoutes.Login, LoginEndpoint.LoginAsync);

        // GET  👉 api/me
        builder.MapGet(WebApiRoutes.Me, MeEndpoint.InfoAsync);
        // POST 👉 api/me/password
        builder.MapPost(WebApiRoutes.MePassword, MeEndpoint.ChangePasswordAsync);

        // POST 👉 api/users/
        builder.MapPost(WebApiRoutes.Users, UserEndpoint.CreateAsync);
        // POST 👉 api/employees/
        builder.MapPost(WebApiRoutes.Employees, EmployeeEndpoint.CreateAsync);
        // POST 👉 api/customers
        builder.MapPost(WebApiRoutes.Customers, CustomerEndpoint.CreateAsync);


        // GET  👉 api/orders/
        builder.MapGet(WebApiRoutes.Orders, OrderEndpoint.GetListAsync);
        // POST 👉 api/order/timing
        builder.MapPost(WebApiRoutes.OrdersTiming, TimingOrderEndpoint.CreateAsync);

        // POST 👉 api/orders/{id}/assign
        builder.MapPost(WebApiRoutes.OrdersIdAssign, OrderEndpoint.AssignEmployeeAsync);

        // POST 👉 api/order/{id}/servicing
        builder.MapPost(WebApiRoutes.OrdersIdServicing, OrderEndpoint.MarkServicingAsync);
        // POST 👉 api/order/{id}/servicing
        builder.MapPost(WebApiRoutes.OrdersIdPaused, OrderEndpoint.MarkPausedAsync);
        // POST 👉 api/order/{id}/servicing
        builder.MapPost(WebApiRoutes.OrdersIdCancelled, OrderEndpoint.MarkCancelledAsync);
        // POST 👉 api/order/{id}/servicing
        builder.MapPost(WebApiRoutes.OrdersIdTerminated, OrderEndpoint.MarkTerminatedAsync);

        // POST 👉 api/order/{id}/timing-consume
        builder.MapPost(WebApiRoutes.OrdersIdTimingConsume, TimingOrderEndpoint.ConsumeAsync);
    }
}