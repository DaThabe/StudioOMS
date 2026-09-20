namespace StudioOMS.Clients;


public readonly record struct ClientCreateDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public readonly record struct ClientCreateResult
{
    public required Guid Id { get; init; }
}