using StudioOMS.Extensions;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Serializer;

namespace StudioOMS;


public interface IOrderClient
{
    Task AssignEmployeeAsync(OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default);
    Task<OrderListResult?> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default);


    Task<OrderCreateResult> CreateTimingAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken = default);
    Task ConsumeTimingAsync(TimingOrderConsume dto, CancellationToken cancellationToken = default);
}

internal sealed class OrderClient(ServerUrl url, HttpClient client, Action<HttpRequestMessage>? messageOptionsAction = null) : IOrderClient
{
    public async Task AssignEmployeeAsync(OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PutJson(url.Orders, dto, AppJsonSerializerContext.Default.OrderAssignEmployeeDto);
        messageOptionsAction?.Invoke(request);

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<OrderListResult?> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.Get(url.GetOderList(dto));
        messageOptionsAction?.Invoke(request);

        return await client.GetJsonAsync(request, AppJsonSerializerContext.Default.OrderListResult, cancellationToken);
    }



    public Task ConsumeTimingAsync(TimingOrderConsume dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<OrderCreateResult> CreateTimingAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}