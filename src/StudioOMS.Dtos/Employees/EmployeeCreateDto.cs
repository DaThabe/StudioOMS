namespace StudioOMS.Employees;


public sealed class EmployeeCreateDto
{
    public required string Name { get; init; }
    public required string Roles { get; init; }
}

public sealed class EmployeeCreateResult
{
    public required string EmployeeId { get; init; }
}