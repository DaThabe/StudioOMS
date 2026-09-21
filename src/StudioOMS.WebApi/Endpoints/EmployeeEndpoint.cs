using StudioOMS.Employees;
using StudioOMS.Messaging;

namespace StudioOMS.Endpoints;


public static class EmployeeEndpoint
{
    public static async Task<IResult> CreateAsync(EmployeeCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<EmployeeCreateRequest, EmployeeId>(EmployeeCreateRequest.FromDto(dto), ct);
        return Results.Ok(id.ToString());
    }
}