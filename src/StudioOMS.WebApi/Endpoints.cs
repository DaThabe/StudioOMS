using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Me;
using StudioOMS.Orders;
using StudioOMS.Session;
using StudioOMS.Users;

namespace StudioOMS;

internal static class Endpoints
{
    public static void MapStudioOMS(this IEndpointRouteBuilder builder)
    {
        // Endpoints
        var apiGroup = builder.MapGroup("/api");
        // api/me
        var meGroup = apiGroup.MapGroup("/me");
        // api/users
        var userGroup = apiGroup.MapGroup("/users");
        // api/employees
        var employeeGroup = apiGroup.MapGroup("/employees");
        // api/customers
        var customerGroup = apiGroup.MapGroup("/customers");
        // api/orders
        var orderGroup = apiGroup.MapGroup("/orders");
        // api/orders/{id}
        var orderIdGroup = orderGroup.MapGroup("/{id:guid}");


        // POST 👉 api/login
        apiGroup.MapPost("/login", LoginEndpoint.LoginAsync);


        // GET  👉 api/me
        meGroup.MapGet("/", MeEndpoint.InfoAsync);
        // POST 👉 api/me/password
        meGroup.MapPost("/password", MeEndpoint.ChangePasswordAsync);
        // POST 👉 api/users/
        userGroup.MapPost("/", UserEndpoint.CreateAsync);
        // POST 👉 api/employees/
        employeeGroup.MapPost("/", EmployeeEndpoint.CreateAsync);
        // POST 👉 api/customers
        customerGroup.MapPost("/", CustomerEndpoint.CreateAsync);


        // GET  👉 api/orders/
        orderGroup.MapGet("/", OrderEndpoint.GetListAsync);
        // POST 👉 api/order/timing
        orderGroup.MapPost("/timing", TimingOrderEndpoint.CreateAsync);

        // POST 👉 api/orders/{id}/assign
        orderIdGroup.MapPost("/assign", OrderEndpoint.AssignEmployeeAsync);

        // POST 👉 api/order/{id}/servicing
        orderIdGroup.MapPost("/servicing", OrderEndpoint.MarkServicingAsync);
        // POST 👉 api/order/{id}/servicing
        orderIdGroup.MapPost("/paused", OrderEndpoint.MarkPausedAsync);
        // POST 👉 api/order/{id}/servicing
        orderIdGroup.MapPost("/cancelled", OrderEndpoint.MarkCancelledAsync);
        // POST 👉 api/order/{id}/servicing
        orderIdGroup.MapPost("/terminated", OrderEndpoint.MarkTerminatedAsync);

        // POST 👉 api/order/{id}/timing-consume
        orderIdGroup.MapPost("/timing-consume", TimingOrderEndpoint.ConsumeAsync);
    }
}