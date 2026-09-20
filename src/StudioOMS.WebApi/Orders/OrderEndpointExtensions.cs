using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Requests;
using StudioOMS.Requests.Orders;

namespace StudioOMS.Orders;


public static class OrderEndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapOrderEndpoints()
        {
            var group = app.MapGroup("/api/orders");

            group.MapPost("/{id:guid}/assign", AssignEmployee);
            group.MapPost("/{id:guid}/servicing", MarkServicing);
            //group.MapPost("/{id:guid}/pause", MarkPaused);
            //group.MapPost("/{id:guid}/terminate", MarkTerminated);
            //group.MapPost("/{id:guid}/cancel", MarkCancelled);

            return app;
        }
    }

    private static async Task<IResult> AssignEmployee(
        Guid id, OrderAssignEmployeeRequest dto, ISender sender, CancellationToken ct)
    {
        await sender.SendAsync(
            new OrderAssignEmployeeRequest(new OrderId(id), new EmployeeId(dto.EmployeeId)), ct);

        return Results.Ok();
    }

    private static async Task<IResult> MarkServicing(
        Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.SendAsync<OrderMarkServicingDto, OrderStateChangeResult>(
            new OrderMarkServicingDto(new OrderId(id)), ct);

        return result.ToHttpResult();
    }


    public static IResult ToHttpResult(this OrderStateChangeResult result) => result switch
    {
        OrderStateChangeResult.SuccessResult => Results.Ok(),
        OrderStateChangeResult.NotAsExpectedResult e => Results.BadRequest(new
        {
            Message = $"当前状态 {e.Actual} 不允许该操作",
            Actual = e.Actual.ToString(),
            Expected = e.Expects.Select(s => s.ToString())
        }),
        _ => Results.Problem("未知的状态变更结果")
    };
}