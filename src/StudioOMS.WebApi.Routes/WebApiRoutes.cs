namespace StudioOMS.WebApi;


public static class WebApiRoutes
{
    public const string Login = "/api/login";

    public const string Me = "/api/me";
    public const string MePassword = "api/me/password";

    public const string Users = "/api/users";
    public const string Employees = "/api/employees/";
    public const string Customers = "/api/customer/";

    public const string Orders = "/api/orders/";
    public const string OrdersTiming = "/api/orders/timing";
    public const string OrdersIdAssign = "/api/orders/{id}/assign";
    public const string OrdersIdServicing = "/api/orders/{id}/servicing";
    public const string OrdersIdPaused = "/api/orders/{id}/paused";
    public const string OrdersIdCancelled = "/api/orders/{id}/cancelled";
    public const string OrdersIdTerminated = "/api/orders/{id}/terminated";
    public const string OrdersIdTimingConsume = "/api/orders/{id}/timing-consume";
}