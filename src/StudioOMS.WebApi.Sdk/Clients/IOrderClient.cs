using StudioOMS.Http;
using StudioOMS.Orders;
using StudioOMS.Routes;
using StudioOMS.Serializer;

namespace StudioOMS.Clients;


public interface IOrderClient
{
    Task AssignEmployeeAsync(Guid orderId, OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default);
    Task<OrderListResult?> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default);


    Task MarkServicingAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task MarkPausedAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task MarkCancelledAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task MarkTerminatedAsync(Guid orderId, CancellationToken cancellationToken = default);



    Task<OrderCreateResult?> CreateTimingAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken = default);
    Task ConsumeTimingAsync(Guid orderId, TimingOrderConsumeDto dto, CancellationToken cancellationToken = default);
}

internal sealed class OrderClient(OrderRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IOrderClient
{
    public Task AssignEmployeeAsync(Guid orderId, OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PutJson(routes.Assign(orderId), dto, DtoJsonSerializerContext.Default.OrderAssignEmployeeDto);
        messageOptionsAction?.Invoke(request);

        return client.SendEnsureSuccessAsync(request, cancellationToken);
    }
    public async Task<OrderListResult?> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Get(routes.List(dto));
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, DtoJsonSerializerContext.Default.OrderListResult, cancellationToken);
    }




    public Task MarkServicingAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Post(routes.MarServicing(orderId));
        messageOptionsAction?.Invoke(request);

        return client.SendEnsureSuccessAsync(request, cancellationToken);
    }

    public Task MarkPausedAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Post(routes.MarkPaused(orderId));
        messageOptionsAction?.Invoke(request);

        return client.SendEnsureSuccessAsync(request, cancellationToken);
    }

    public Task MarkCancelledAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Post(routes.MarkCancelle(orderId));
        messageOptionsAction?.Invoke(request);

        return client.SendEnsureSuccessAsync(request, cancellationToken);
    }

    public Task MarkTerminatedAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Post(routes.MarkTerminated(orderId));
        messageOptionsAction?.Invoke(request);

        return client.SendEnsureSuccessAsync(request, cancellationToken);
    }






    public async Task<OrderCreateResult?> CreateTimingAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken)
    {
        var request = HttpRequestMessage.PostJson(routes.CreateTiming, dto, DtoJsonSerializerContext.Default.TimingOrderCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, DtoJsonSerializerContext.Default.OrderCreateResult, cancellationToken);
    }
    public Task ConsumeTimingAsync(Guid orderId, TimingOrderConsumeDto dto, CancellationToken cancellationToken)
    {
        var request = HttpRequestMessage.PostJson(routes.ConsumeTiming(orderId), dto, DtoJsonSerializerContext.Default.TimingOrderConsumeDto);
        messageOptionsAction?.Invoke(request);

        return client.SendEnsureSuccessAsync(request, cancellationToken);
    }
}