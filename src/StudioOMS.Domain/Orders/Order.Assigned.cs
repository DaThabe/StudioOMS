namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial record class Order
{
    private readonly HashSet<EmployeeId> _assignedEmployees = [];

    /// <summary>
    /// 服务中的员工
    /// </summary>
    public IReadOnlySet<EmployeeId> AssignedEmployees => _assignedEmployees.AsReadOnly();


    /// <summary>
    /// 分配服务员工
    /// </summary>
    public void AssignEmployees(params IEnumerable<EmployeeId> employees)
    {
        if (State is OrderState.Completed or OrderState.Terminated or OrderState.Cancelled)
            throw new InvalidOperationException($"订单状态 {State} 不能派发员工");

        _assignedEmployees.UnionWith(employees);
    }
}