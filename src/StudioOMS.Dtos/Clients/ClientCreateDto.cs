namespace StudioOMS.Clients;


public readonly record struct ClientCreateDto
{
    public required string Name { get; init; }
}

public readonly record struct ClientCreateResult
{
    public required string Id { get; init; }
}