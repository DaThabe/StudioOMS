namespace StudioOMS.Me;


public readonly record struct InfoResult
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string EmployeeId { get; init; }
    public required string EmployeeName { get; init; }
}