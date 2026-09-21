using StudioOMS.Customers;

namespace StudioOMS;

public interface ICustomerClient
{
    ValueTask<CustomerCreateResult> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default);
    ValueTask RenamAsync(CustomerRenameDto dto, CancellationToken cancellationToken = default);
}
