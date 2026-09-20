using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Messaging;

namespace StudioOMS.Orders.Timing;


public static class TimingOrderEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapTimingOrderEndpoints()
        {
            var group = app.MapGroup("/api/orders/timing");

            group.MapPost("/", Create);
            group.MapPost("/{id:guid}/consume", Consume);

            return app;
        }
    }


    private static async Task<IResult> Create(
        TimingOrderCreateRequest dto,
        ISender sender,
        CancellationToken ct)
    {
        var request = new TimingOrderCreateRequest(
            new OrderId(dto.OrderId),
            new ClientId(dto.ClientId),
            new EmployeeId(dto.SalespersonId),
            dto.TotalDays,
            DateTimeOffset.Now
            );

        var orderId = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(request, ct);
        return Results.Ok(orderId.ToString());
    }
    private static async Task<IResult> Consume(
        Guid id,
        TimingOrderConsumeRequest dto,
        ISender sender,
        CancellationToken ct)
    {
        var request = new TimingOrderConsumeRequest(
            new OrderId(id),
            new EmployeeId(dto.EmployeeId),
            ConsumeId.Create(),
            dto.Days,
            DateTime.UtcNow);

        var result = await sender.SendAsync<TimingOrderConsumeRequest, TimingOrderConsumeResult>(request, ct);
        return Results.Ok(result);
    }
}