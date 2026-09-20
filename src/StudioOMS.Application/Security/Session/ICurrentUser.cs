using StudioOMS.Employees;

namespace StudioOMS.Security.Session;


/// <summary>
/// 当前用户
/// </summary>
public interface ICurrentUser
{
    EmployeeId EmployeeId { get; set; }
}