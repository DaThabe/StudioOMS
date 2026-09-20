namespace StudioOMS.Employees;


public sealed class EmployeeCreateDto
{
    public required string Name { get; init; }
    public required EmployeeRole[] Roles { get; init; }
}