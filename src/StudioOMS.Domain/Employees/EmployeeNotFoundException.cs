namespace StudioOMS.Employees;


public abstract class EmployeeException : DomainException
{
    public EmployeeId EmployeeId { get; }


    protected EmployeeException(EmployeeId employeeId)
    {
        EmployeeId = employeeId;
    }
    protected EmployeeException(string message, EmployeeId employeeId)
    {
        EmployeeId = employeeId;
    }
}

/// <summary>
/// 员工不存在异常
/// </summary>
public sealed class EmployeeNotFoundException(EmployeeId employeeId) :
    EmployeeException($"员工 {employeeId} 不存在", employeeId);
