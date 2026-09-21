using StudioOMS.Employees;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Employees;


public sealed class EmployeeCreateRequest : IRequest<EmployeeId>
{
    public required string Name { get; init; }
    public required EmployeeRole[] Roles { get; init; }


    public static EmployeeCreateRequest FromDto(EmployeeCreateDto dto)
    {
        return new()
        {
            Name = dto.Name,
            Roles = dto.Roles
        };
    }

    internal sealed class Handler(
            IEmployeeRepository employeeRepository
        ) : IRequestHandler<EmployeeCreateRequest, EmployeeId>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.EmployeeCreate);

        public async ValueTask<EmployeeId> HandleAsync(EmployeeCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = Employee.Create(request.Roles);
            entity.Rename(request.Name);
            await employeeRepository.SaveAsync(entity, cancellationToken);

            return entity.Id;
        }
    }
}