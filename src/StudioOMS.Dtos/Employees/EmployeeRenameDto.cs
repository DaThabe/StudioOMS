namespace StudioOMS.Clients;


public readonly record struct EmployeeRenameDto
{
    public required string Name { get; init; }
}