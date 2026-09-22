using StudioOMS.Messaging;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Orders;


public static class OrderEndpoint
{
    public static async Task<IResult> AssignEmployeeAsync(Guid id, OrderAssignEmployeeDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(dto.ToRequest(id), ct);
        return Results.Ok();
    }

    public static async Task<IResult> MarkServicingAsync(string id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(id.ToOrderMarkServicingRequest(), ct);
        return Results.Ok();
    }
    public static async Task<IResult> MarkPausedAsync(string id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(id.ToOrderMarkPausedRequest(), ct);
        return Results.Ok();
    }
    public static async Task<IResult> MarkCancelledAsync(string id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(id.ToOrderMarkCancelledRequest(), ct);
        return Results.Ok();
    }
    public static async Task<IResult> MarkTerminatedAsync(string id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(id.ToOrderMarkTerminatedRequest(), ct);
        return Results.Ok();
    }



    public static async Task<IResult> GetListAsync([AsParameters] OrderListDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var response = await sender.SendAsync<OrderListRequest, OrderListResponse>(dto.ToRequest(), ct);
        return Results.Ok(response.ToOrderListResult());
    }
}

public static class TimingOrderEndpoint
{
    public static async Task<IResult> CreateAsync(TimingOrderCreateDto dto,
       ISender sender,
       CancellationToken ct)
    {
        var response = await sender.SendAsync<TimingOrderCreateRequest, OrderId>(dto.ToRequest(), ct);
        return Results.Ok(response.ToOrderCreateResult());
    }

    public static async Task<IResult> ConsumeAsync(Guid id, TimingOrderConsumeDto dto,
        ISender sender,
        CancellationToken ct)
    {
        await sender.SendAsync(dto.ToRequest(id), ct);
        return Results.Ok();
    }
}