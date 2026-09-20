using StudioOMS.Orders;
using StudioOMS.Requests;
using StudioOMS.Requests.Orders;

namespace StudioOMS.Endpoints.Orders;


public static class OrderEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapOrderEndpoints()
        {
            var group = app.MapGroup("/api/orders");

            group.MapPost("/{id:guid}/assign", AssignEmployeeAsync);
            group.MapPost("/{id:guid}/servicing", MarkServicingAsync);
            //group.MapPost("/{id:guid}/pause", MarkPaused);
            //group.MapPost("/{id:guid}/terminate", MarkTerminated);
            //group.MapPost("/{id:guid}/cancel", MarkCancelled);

            return app;
        }
    }

    private static async Task<IResult> AssignEmployeeAsync(Guid id, OrderAssignEmployeeDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(OrderAssignEmployeeRequest.FromDto(id, dto), ct);
        return Results.Ok();
    }

    private static async Task<IResult> MarkServicingAsync(string id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(OrderMarkServicingRequest.FromOrderId(id), ct);
        return Results.Ok();
    }
}