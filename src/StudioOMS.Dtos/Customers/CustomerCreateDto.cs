namespace StudioOMS.Customers;


public readonly record struct CustomerCreateDto
{
    public required string Name { get; init; }
}

public readonly record struct CustomerCreateResult
{
    public required string CustomerId { get; init; }
}