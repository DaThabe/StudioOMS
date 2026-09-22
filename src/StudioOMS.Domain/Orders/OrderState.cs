namespace StudioOMS.Orders;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderState
{
    /// <summary>
    /// 等待开始 - 创建了还没开始
    /// </summary>
    Waiting = 0,

    /// <summary>
    /// 服务中 - 正常服务中
    /// </summary>
    Servicing = 1,

    /// <summary>
    /// 已暂停 - 暂时停止
    /// </summary>
    Paused = 2,

    /// <summary>
    /// 已完成 - 正常完成的
    /// </summary>
    Completed = 3,

    /// <summary>
    /// 已取消 - 还没开始就要结束的
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// 已终止 - 中途开始结束的
    /// </summary>
    Terminated = 5,
}
