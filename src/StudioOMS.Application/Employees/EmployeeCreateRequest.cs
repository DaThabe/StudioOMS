using StudioOMS.Messaging;
using StudioOMS.Security.Permission;

namespace StudioOMS.Employees;


public sealed class EmployeeCreateRequest : IRequest<EmployeeId>
{
    public required EmployeeName Name { get; init; }
    public required IReadOnlySet<EmployeeRole> Roles { get; init; }


    internal sealed class Handler(
            IEmployeeRepository employeeRepository
        ) : IRequestHandler<EmployeeCreateRequest, EmployeeId>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.EmployeeCreate);

        public async ValueTask<EmployeeId> HandleAsync(EmployeeCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = Employee.Create(request.Name, request.Roles);
            await employeeRepository.SaveAsync(entity, cancellationToken);

            return entity.Id;
        }
    }
}