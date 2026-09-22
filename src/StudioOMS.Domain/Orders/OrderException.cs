namespace StudioOMS.Orders;

public abstract class OrderException : StudioOMSException
{
    public OrderId OrderId { get; }


    protected OrderException(OrderId orderId) =>
         OrderId = orderId;
    protected OrderException(string message, OrderId orderId) : base(message) =>
        OrderId = orderId;



    protected static string GetStateName(OrderState state) => state switch
    {
        OrderState.Waiting => "未开始服务",
        OrderState.Paused => "已暂停",
        OrderState.Completed => "已完成",
        OrderState.Terminated => "已终止",
        OrderState.Cancelled => "已取消",
        _ => "未知状态"
    };
}