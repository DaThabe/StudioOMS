using StudioOMS.Messaging;
using StudioOMS.Security.Permission;

namespace StudioOMS.Customers;


public sealed class CustomerCreateRequest : IRequest<CustomertId>
{
    public required string Name { get; init; }


    public static CustomerCreateRequest FromDto(CustomerCreateDto dto)
    {
        return new() { Name = dto.Name };
    }


    internal sealed class Handler(
            ICustomerRepository repository
        ) : IRequestHandler<CustomerCreateRequest, CustomertId>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.CustomerCreate);

        public async ValueTask<CustomertId> HandleAsync(CustomerCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = Customer.Create();
            entity.Rename(request.Name);
            await repository.SaveAsync(entity, cancellationToken);

            return entity.Id;
        }
    }
}