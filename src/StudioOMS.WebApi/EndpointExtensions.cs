using StudioOMS.Endpoints;

namespace StudioOMS;

internal static class EndpointExtensions
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


        // POST 👉api/me/password
        meGroup.MapPost("/password", MeEndpoint.ChangePasswordAsync);
        // POST 👉api/users/
        userGroup.MapPost("/", UserEndpoint.CreateAsync);
        // POST 👉api/employees/
        employeeGroup.MapPost("/", EmployeeEndpoint.CreateAsync);
        // POST 👉api/customers
        customerGroup.MapPost("/", CustomerEndpoint.CreateAsync);
        // GET  👉api/orders/
        orderGroup.MapGet("/", OrderEndpoint.GetListAsync);
        // POST 👉api/orders/assign
        orderIdGroup.MapPost("/assign", OrderEndpoint.AssignEmployeeAsync);
        // POST 👉api/order/{id}/servicing
        orderIdGroup.MapPost("/servicing", OrderEndpoint.MarkServicingAsync);
        // POST 👉api/order/{id}/consume-timing
        orderIdGroup.MapPost("/consume-timing", TimingOrderEndpoint.ConsumeAsync);
    }
}