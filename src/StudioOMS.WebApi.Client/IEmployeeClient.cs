using StudioOMS.Employees;

namespace StudioOMS;

public interface IEmployeeClient
{

    ValueTask<EmployeeCreateResult> ECreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);
    ValueTask RenamAsync(EmployeeRenameDto dto, CancellationToken cancellationToken = default);
}
