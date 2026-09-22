using StudioOMS.Messaging;
using StudioOMS.Permission;

namespace StudioOMS.Employees;


public sealed class EmployeeRenameRequest : IRequest
{
    public required EmployeeId Id { get; init; }
    public required EmployeeName Name { get; init; }


    internal sealed class Handler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeRenameRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.EmployeeManage);

        public async ValueTask HandleAsync(EmployeeRenameRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await employeeRepository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"员工 [{request.Id}] 不存在");

            entity.Rename(request.Name);
            await employeeRepository.SaveAsync(entity, cancellationToken);
        }
    }
}