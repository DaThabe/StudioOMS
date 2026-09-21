using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Endpoints;


public static class OrderEndpoint
{
    public static async Task<IResult> AssignEmployeeAsync(Guid id, OrderAssignEmployeeDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(OrderAssignEmployeeRequest.FromDto(id, dto), ct);
        return Results.Ok();
    }

    public static async Task<IResult> MarkServicingAsync(string id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(OrderMarkServicingRequest.FromOrderId(id), ct);
        return Results.Ok();
    }

    public static async Task<IResult> GetListAsync([AsParameters] OrderListDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.SendAsync<OrderListRequest, OrderListResult>(OrderListRequest.FromDto(dto), ct);
        return Results.Ok(result);
    }
}

public static class TimingOrderEndpoint
{
    public static async Task<IResult> CreateAsync(TimingOrderCreateDto dto,
       ISender sender,
       CancellationToken ct)
    {
        var id = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(TimingOrderCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }

    public static async Task<IResult> ConsumeAsync(Guid id, TimingOrderConsumeDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(TimingOrderConsumeRequest.FromDto(id, dto), ct);
        return Results.Ok();
    }
}