using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Endpoints;


public static class OrderEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapOrderEndpoints()
        {
            var group = app.MapGroup("/api/orders");

            group.MapGet("/", GetListAsync);
            group.MapPost("/{id:guid}/assign", AssignEmployeeAsync);
            group.MapPost("/{id:guid}/servicing", MarkServicingAsync);
            //group.MapPost("/{id:guid}/pause", MarkPaused);
            //group.MapPost("/{id:guid}/terminate", MarkTerminated);
            //group.MapPost("/{id:guid}/cancel", MarkCancelled);


            var timingGroup = app.MapGroup("/timing");
            timingGroup.MapPost("/", TimingCreateAsync);
            timingGroup.MapPost("/{id:guid}/consume", TimingConsumeAsync);

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

    private static async Task<IResult> GetListAsync([AsParameters] OrderListDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.SendAsync<OrderListRequest, OrderListResult>(OrderListRequest.FromDto(dto), ct);
        return Results.Ok(result);
    }


    private static async Task<IResult> TimingCreateAsync(TimingOrderCreateDto dto,
       ISender sender,
       CancellationToken ct)
    {
        var id = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(TimingOrderCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
    private static async Task<IResult> TimingConsumeAsync(Guid id, TimingOrderConsumeDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(TimingOrderConsumeRequest.FromDto(id, dto), ct);
        return Results.Ok();
    }
}