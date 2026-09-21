namespace StudioOMS.Security.Permission;


public enum PermissionType
{
    // 订单
    OrderRead,
    OrderCreate,
    OrderAssign,
    OrderConsume,
    OrderManage,      // 开始服务、暂停、终止、取消


    // 客户
    CustomerRead,
    CustomerCreate,
    CustomerManage,


    // 员工
    EmployeeRead,
    EmployeeCreate,
    EmployeeManage,   // 修改、停用


    // 用户
    UserRead,
    UserCreate,
    UserManage,       // 修改、禁用、重置密码


    // 财务
    FinanceRead,      // 查看结算汇总
    FinanceSettle     // 执行结算
}


public static class PermissionTypeExtensions
{
    extension(PermissionType)
    {
        public static IReadOnlySet<PermissionType> Group(params IEnumerable<PermissionType> permissions)
        {
            return new HashSet<PermissionType>(permissions);
        }
    }
}