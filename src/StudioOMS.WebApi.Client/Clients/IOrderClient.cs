using StudioOMS.Extensions;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Routes;
using StudioOMS.Serializer;

namespace StudioOMS.Clients;


public interface IOrderClient
{
    Task AssignEmployeeAsync(Guid orderId, OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default);
    Task<OrderListResult?> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default);
}

public interface ITimingOrderClient : IOrderClient
{
    Task<OrderCreateResult?> CreateAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken = default);
    Task ConsumeAsync(Guid orderId, TimingOrderConsumeDto dto, CancellationToken cancellationToken = default);
}


internal sealed class OrderClient(OrderRoutes routes, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IOrderClient, ITimingOrderClient
{
    public async Task AssignEmployeeAsync(Guid orderId, OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PutJson(routes.Assign(orderId), dto, AppJsonSerializerContext.Default.OrderAssignEmployeeDto);
        messageOptionsAction?.Invoke(request);

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    public async Task<OrderListResult?> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Get(routes.List(dto));
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, AppJsonSerializerContext.Default.OrderListResult, cancellationToken);
    }



    async Task<OrderCreateResult?> ITimingOrderClient.CreateAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken)
    {
        var request = HttpRequestMessage.PostJson(routes.CreateTiming, dto, AppJsonSerializerContext.Default.TimingOrderCreateDto);
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, AppJsonSerializerContext.Default.OrderCreateResult, cancellationToken);
    }
    async Task ITimingOrderClient.ConsumeAsync(Guid orderId, TimingOrderConsumeDto dto, CancellationToken cancellationToken)
    {
        var request = HttpRequestMessage.PostJson(routes.ConsumeTiming(orderId), dto, AppJsonSerializerContext.Default.TimingOrderConsumeDto);
        messageOptionsAction?.Invoke(request);

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}