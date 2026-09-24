namespace StudioOMS;


public static class ErrorCodes
{
    public const string NotLogin = "NOT_LOGIN";


    // 订单
    public static class Order
    {
        /// <summary>当前状态禁止操作</summary>
        public const string CurrentStateOperationNotAllowed = "CURRENT_STATE_OPERATION_NOT_ALLOWED";

        /// <summary>当前状态无法转换到目标状态</summary>
        public const string StateTransitionNotAllowed = "STATE_TRANSITION_NOT_ALLOWED";

        /// <summary>状态转换时间无效</summary>
        public const string StateTransitionTimeInvalid = "STATE_TRANSITION_TIME_INVALID";



        /// <summary>划扣的员工不在订单服务名单中</summary>
        public const string ConsumeEmployeeNotAssigned = "CONSUME_EMPLOYEE_NOT_ASSIGNED";

        /// <summary>划扣超过上限</summary>
        public const string ConsumeExceedsLimit = "CONSUME_EXCEEDS_LIMIT";

        /// <summary>划扣超过上限</summary>
        public const string ConsumeTimeInvalid = "CONSUME_TIME_INVALID";
    }
}