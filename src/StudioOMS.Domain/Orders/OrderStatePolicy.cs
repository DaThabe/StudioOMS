using StudioOMS.Employees;

namespace StudioOMS.Orders;


public static class OrderStatePolicy
{
    /// <summary>
    /// 终止订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateOperationPermissionException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    public static void MarkServicing(Order order, Employee employee, DateTimeOffset timestamp)
    {
        ThrowIfOrderStateOperationPermissionException(order, employee);

        order.MarkServicing(employee.Id, timestamp);
    }

    /// <summary>
    /// 终止订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateOperationPermissionException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    public static void MarkPaused(Order order, Employee employee, DateTimeOffset timestamp)
    {
        ThrowIfOrderStateOperationPermissionException(order, employee);

        order.MarkPaused(employee.Id, timestamp);
    }

    /// <summary>
    /// 终止订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateOperationPermissionException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    public static void MarkCancelled(Order order, Employee employee, DateTimeOffset timestamp)
    {
        ThrowIfOrderStateOperationPermissionException(order, employee);

        order.MarkCancelled(employee.Id, timestamp);
    }

    /// <summary>
    /// 终止订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateOperationPermissionException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    public static void MarkTerminated(Order order, Employee employee, DateTimeOffset timestamp)
    {
        ThrowIfOrderStateOperationPermissionException(order, employee);

        order.MarkTerminated(employee.Id, timestamp);
    }


    private static void ThrowIfOrderStateOperationPermissionException(Order order, Employee employee)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(employee);

        var roles = employee.Roles;

        if (!employee.Roles.Contains(EmployeeRole.Admin) && !roles.Contains(EmployeeRole.DesignSupervisor))
            throw new OrderStateOperationPermissionException(order.Id, order.State, employee.Id);
    }
}