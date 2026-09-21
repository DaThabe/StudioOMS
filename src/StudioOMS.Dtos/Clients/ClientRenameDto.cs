namespace StudioOMS.Clients;


public readonly record struct ClientRenameDto
{
    public required string Name { get; init; }
}