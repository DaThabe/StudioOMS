namespace StudioOMS.Employees;


public sealed class Employee : Entity<EmployeeId>
{
    public string Name { get; private set; } = "未命名员工";



    public void Rename(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var trimmed = value.Trim();

        if (string.Equals(trimmed, Name, StringComparison.OrdinalIgnoreCase))
            return;

        Name = trimmed;
    }



    internal Employee() { }
    public static Employee Create(EmployeeId employee)
    {
        if (employee == EmployeeId.Empty)
            throw new ArgumentException("员工 Id 不可为空", nameof(employee));


        return new()
        {
            Id = employee
        };
    }
}