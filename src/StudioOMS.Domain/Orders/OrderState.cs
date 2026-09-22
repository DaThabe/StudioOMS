namespace StudioOMS.Orders;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderState
{
    /// <summary>
    /// 等待开始
    /// </summary>
    Waiting = 0,

    /// <summary>
    /// 服务中
    /// </summary>
    Servicing = 1,

    /// <summary>
    /// 已暂停
    /// </summary>
    Paused = 2,

    /// <summary>
    /// 已完成
    /// </summary>
    Completed = 3,

    /// <summary>
    /// 已终止
    /// </summary>
    Terminated = 4,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 5
}
