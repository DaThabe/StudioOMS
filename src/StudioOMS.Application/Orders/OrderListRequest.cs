using StudioOMS.Messaging;
using StudioOMS.Security.Permission;

namespace StudioOMS.Orders;


public sealed class OrderListRequest : IRequest<OrderListResult>
{
    public required int Skip { get; init; }
    public required int Take { get; init; }
    public IReadOnlySet<OrderType> Types { get; init; } = new HashSet<OrderType>();


    private OrderListRequest() { }
    public static OrderListRequest FromDto(OrderListDto dto)
    {
        return new()
        {
            Skip = dto.Skip ?? 0,
            Take = dto.Take ?? 20,
            Types = ParseTypes(dto.Types)
        };

        static HashSet<OrderType> ParseTypes(string? typesString)
        {
            var typeStringArr = typesString?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (typeStringArr is null || typeStringArr.Length == 0) return [];

            var typeSet = new HashSet<OrderType>();
            foreach (var typeString in typeStringArr)
            {
                if (!Enum.TryParse<OrderType>(typeString, true, out var result))
                    throw new ArgumentException($"非法的订单类型：{typeString}");

                typeSet.Add(result);
            }

            return typeSet;
        }
    }

    internal sealed class Handler(
            IOrderQuery orderQuery
        ) : IRequestHandler<OrderListRequest, OrderListResult>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.OrderRead);

        public ValueTask<OrderListResult> HandleAsync(OrderListRequest request,
            CancellationToken cancellationToken = default)
        {
            return orderQuery.QueryAsync(request, cancellationToken);
        }
    }
}

public enum OrderType
{
    Timing,
    Counting
}


public interface IOrderQuery
{
    ValueTask<OrderListResult> QueryAsync(OrderListRequest requesst, CancellationToken cancellationToken = default);
}