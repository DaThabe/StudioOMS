namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract record class Order : IEquatable<Order>
{
    public required OrderId Id { get; init; }
    public required ClientId ClientId { get; init; }
    public required EmployeeId SalespersonId { get; init; }

    public OrderState State { get; private set; } = OrderState.Waiting;
    public string Title { get; private set; } = string.Empty;


    public void ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException("订单名称不可为空");

        var trimmed = title.Trim();
        if (Title.Equals(trimmed)) return;

        Title = trimmed;
    }

    public MarkStateResult MarkServiceing() =>
        TransitionTo(OrderState.Servicing, OrderState.Waiting, OrderState.Paused);

    public MarkStateResult MarkPaused() =>
        TransitionTo(OrderState.Paused, OrderState.Servicing, OrderState.Waiting);



    public virtual bool Equals(Order? other) => Id.Equals(other?.Id);
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Id.ToString() ?? string.Empty;




    private MarkStateResult TransitionTo(OrderState target, params IEnumerable<OrderState> allowedFrom)
    {
        if (State == target)
            return MarkStateResult.Success(State);

        if (!allowedFrom.Contains(State))
            return MarkStateResult.NotAsExpected(State, allowedFrom);

        State = target;
        return MarkStateResult.Success(State);
    }
}


public readonly record struct OrderId
{
    public static OrderId Empty => default;

    private readonly Guid _value;
    private OrderId(Guid value) => _value = value;
    public override string ToString() => _value.ToString("N");


    public static OrderId Create() => new(Guid.CreateVersion7());
}