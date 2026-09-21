using StudioOMS.Messaging;

namespace StudioOMS.Employees;


public static class EmployeeEndpoint
{
    public static async Task<IResult> CreateAsync(EmployeeCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<EmployeeCreateRequest, EmployeeId>(dto.ToRequest(), ct);
        return Results.Ok(id.ToEmployeeCreateResult());
    }
}