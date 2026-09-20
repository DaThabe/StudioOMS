using StudioOMS.Employees;
using StudioOMS.Requests;
using StudioOMS.Requests.Employees;

namespace StudioOMS.Endpoints.Employees;


public static class EmployeeEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapEmployeeEndpoints()
        {
            var group = app.MapGroup("/api/employees");

            group.MapPost("/", CreateAsync);

            return app;
        }
    }

    private static async Task<IResult> CreateAsync(EmployeeCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<EmployeeCreateRequest, EmployeeId>(EmployeeCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
}