using StudioOMS.Employees;
using StudioOMS.Mappers;
using StudioOMS.Messaging;

namespace StudioOMS.Endpoints;


public static class EmployeeEndpoint
{
    public static async Task<IResult> CreateAsync(EmployeeCreateDto dto,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.SendAsync<EmployeeCreateRequest, EmployeeId>(dto.ToRequest(), ct);
        return ResponseResults.Ok(id.ToEmployeeCreateResult());
    }
}