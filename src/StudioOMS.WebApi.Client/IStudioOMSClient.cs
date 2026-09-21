using StudioOMS.Login;

namespace StudioOMS;


public interface IStudioOMSClient
{
    ValueTask<LoginResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);


    IUserClient User { get; }
    IEmployeeClient Employee { get; }
    ICustomerClient Customer { get; }
    IOrderClient Order { get; }
}