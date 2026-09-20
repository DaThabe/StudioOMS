namespace StudioOMS.Employees;


public interface IEmployeeRepository : IRepository<Employee, EmployeeId>
{
    ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default);
}