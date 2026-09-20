namespace StudioOMS.Employees;


public sealed class EmployeeCreateDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}