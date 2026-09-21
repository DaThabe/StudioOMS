using StudioOMS.Customers;
using StudioOMS.Messaging;

namespace StudioOMS.Endpoints;


internal static class CustomerEndpoint
{
    public static async Task<IResult> CreateAsync(CustomerCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<CustomerCreateRequest, CustomertId>(CustomerCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
}



internal static class EndpointExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapStudioOMS()
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
}