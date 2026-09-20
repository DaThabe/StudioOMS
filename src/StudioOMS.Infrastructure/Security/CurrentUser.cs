using StudioOMS.Employees;
using StudioOMS.Security.Session;

namespace StudioOMS.Security;


internal sealed class CurrentUser : ICurrentUser
{
    public EmployeeId EmployeeId { get; set; }
}