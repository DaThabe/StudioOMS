namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial class Order
{
    public required Money Price { get; init; }
}