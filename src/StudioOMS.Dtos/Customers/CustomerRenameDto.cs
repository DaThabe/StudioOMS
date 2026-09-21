namespace StudioOMS.Customers;


public readonly record struct CustomerRenameDto
{
    public required string Name { get; init; }
}