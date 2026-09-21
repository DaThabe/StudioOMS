using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders.Timing;

namespace StudioOMS.Orders;

public static class OrderMapper
{
    public static OrderListRequest ToRequest(this OrderListDto dto)
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
    public static OrderListResult ToOrderListResult(this OrderListResponse response) => new()
    {
        Items = [.. response.Items.Select(x => new OrderListItemDto()
        {
            Id = x.Id.ToString(),
            Title = x.Title,
            State = x.State.ToString(),
            CreateAt = x.CreateAt,
            CustomerId = x.CustomerId.ToString(),
            CustomerName = x.CustomerName.ToString()
        })]
    };


    public static OrderAssignEmployeeRequest ToRequest(this OrderAssignEmployeeDto dto, Guid orderId) => new()
    {
        Id = OrderId.From(orderId),
        EmployeeId = EmployeeId.Parse(dto.EmployeeId)
    };
    public static OrderMarkServicingRequest ToOrderMarkServicingRequest(this string orderId) => new()
    {
        OrderId = OrderId.Parse(orderId)
    };
    public static OrderCreateResult ToOrderCreateResult(this OrderId id) => new()
    {
        OrderId = id.ToString()
    };


    public static TimingOrderCreateRequest ToRequest(this TimingOrderCreateDto dto) => new()
    {
        CustomerId = CustomertId.Parse(dto.CustomerId),
        SalespersonId = EmployeeId.Parse(dto.SalespersonId),
        TotalDays = dto.TotalDays,
        Title = dto.Title
    };
    public static TimingOrderConsumeRequest ToRequest(this TimingOrderConsumeDto dto, Guid orderId) => new()
    {
        ConsuemDays = dto.Days,
        EmployeeId = EmployeeId.Parse(dto.EmployeeId),
        OrderId = OrderId.From(orderId)
    };
}