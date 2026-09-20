using StudioOMS.Employees;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Employees;


public sealed class EmployeeCreateRequest : IRequest
{
    public required EmployeeId Id { get; init; }
    public required string Name { get; init; }
    public required EmployeeRole[] Roles { get; init; }


    public static implicit operator EmployeeCreateRequest(EmployeeCreateDto dto)
    {
        return new()
        {
            Id = new EmployeeId(dto.Id),
            Name = dto.Name,
            Roles = dto.Roles
        };
    }

    internal sealed class Handler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeCreateRequest>, IRequirePermissions
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } = PermissionType.Group(PermissionType.EmployeeCreate);

        public async ValueTask HandleAsync(EmployeeCreateRequest request, CancellationToken cancellationToken = default)
        {
            var user = Employee.Create(request.Id, request.Roles);
            user.Rename(request.Name);

            await employeeRepository.SaveAsync(user, cancellationToken);
        }
    }
}