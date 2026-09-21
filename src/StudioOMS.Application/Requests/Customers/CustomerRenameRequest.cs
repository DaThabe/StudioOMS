using StudioOMS.Customers;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Customers;


public sealed class CustomerRenameRequest : IRequest
{
    public required CustomertId Id { get; init; }
    public required string Name { get; init; }


    public static CustomerRenameRequest FromDto(Guid customerId, CustomerRenameDto dto)
    {
        return new()
        {
            Id = new(customerId),
            Name = dto.Name
        };
    }

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