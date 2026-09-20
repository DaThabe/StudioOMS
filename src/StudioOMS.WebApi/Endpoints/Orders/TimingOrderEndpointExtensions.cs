using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Requests;
using StudioOMS.Requests.Orders.Timing;

namespace StudioOMS.Endpoints.Orders;


public static class TimingOrderEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapTimingOrderEndpoints()
        {
            var group = app.MapGroup("/api/orders/timing");

            group.MapPost("/", CreateAsync);
            group.MapPost("/{id:guid}/consume", ConsumeAsync);

            return app;
        }
    }


    private static async Task<IResult> CreateAsync(TimingOrderCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(TimingOrderCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
    private static async Task<IResult> ConsumeAsync(Guid id, TimingOrderConsumeDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(TimingOrderConsumeRequest.FromDto(id, dto), ct);
        return Results.Ok();
    }
}