namespace StudioOMS.Orders;


/// <summary>
/// 订单状态
/// </summary>
public enum OrderState
{
    /// <summary>
    /// 等待开始
    /// </summary>
    Waiting,

    /// <summary>
    /// 服务中
    /// </summary>
    Servicing,

    /// <summary>
    /// 已暂停
    /// </summary>
    Paused,

    /// <summary>
    /// 已完成
    /// </summary>
    Completed,

    /// <summary>
    /// 已终止
    /// </summary>
    Terminated,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled
}