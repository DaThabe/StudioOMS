using StudioOMS.Customers;
using StudioOMS.Employees;

namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial class Order : Entity<OrderId>
{
    /// <summary>
    /// 客户Id
    /// </summary>
    public required CustomertId CustomerId { get; init; }
    /// <summary>
    /// 销售员Id
    /// </summary>
    public required EmployeeId SalespersonId { get; init; }



    /// <summary>
    /// 创建时间
    /// </summary>
    public required DateTimeOffset CreateAt { get; init; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; private set; } = "未命名订单";


    /// <summary>
    /// 更改标题
    /// </summary>
    /// <exception cref="ArgumentException" />
    public void ChangeTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        // 没变化
        var trimmed = title.Trim();
        if (string.Equals(Title, trimmed)) return;

        // 更新
        Title = trimmed;
    }
}