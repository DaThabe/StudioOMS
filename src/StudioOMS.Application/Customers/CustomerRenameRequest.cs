using StudioOMS.Messaging;
using StudioOMS.Permission;

namespace StudioOMS.Customers;


public sealed class CustomerRenameRequest : IRequest
{
    public required CustomertId Id { get; init; }
    public required CustomerName Name { get; init; }


    internal sealed class Handler(
            ICustomerRepository repository
        ) : IRequestHandler<CustomerRenameRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.CustomerManage);

        public async ValueTask HandleAsync(CustomerRenameRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = await repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"客户 [{request.Id}] 不存在");

            entity.Rename(request.Name);
            await repository.SaveAsync(entity, cancellationToken);
        }
    }
}