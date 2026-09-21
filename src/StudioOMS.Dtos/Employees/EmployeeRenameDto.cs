namespace StudioOMS.Employees;


public readonly record struct EmployeeRenameDto
{
    public required string Name { get; init; }
}