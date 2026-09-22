using StudioOMS.Employees;

namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial class Order
{
    private readonly HashSet<EmployeeId> _assignedEmployees = [];

    /// <summary>
    /// 服务中的员工
    /// </summary>
    public IReadOnlySet<EmployeeId> AssignedEmployees => _assignedEmployees.AsReadOnly();


    /// <summary>
    /// 分配员工来服务订单
    /// </summary>
    /// <exception cref="OrderStateOperationException"></exception>
    public void AssignEmployees(params IEnumerable<EmployeeId> employees)
    {
        if (State is OrderState.Waiting or OrderState.Servicing or OrderState.Paused)
            _assignedEmployees.UnionWith(employees);

        throw new OrderStateOperationException(Id, State, OrderStateOperationType.AssignedEmployee);
    }
}