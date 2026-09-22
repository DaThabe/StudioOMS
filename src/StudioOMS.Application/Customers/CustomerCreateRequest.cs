using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Permission;

namespace StudioOMS.Customers;


public sealed class CustomerCreateRequest : IRequest<CustomertId>
{
    public required CustomerName Name { get; init; }


    internal sealed class Handler(
            ICustomerRepository repository
        ) : IRequestHandler<CustomerCreateRequest, CustomertId>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.CustomerCreate);

        public async ValueTask<CustomertId> HandleAsync(CustomerCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = Customer.Create(request.Name);
            await repository.SaveAsync(entity, cancellationToken);

            return entity.Id;
        }
    }
}