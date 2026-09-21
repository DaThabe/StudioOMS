using StudioOMS.Clients;
using StudioOMS.Extensions;
using StudioOMS.Login;
using StudioOMS.Serializer;
using StudioOMS.Users;

namespace StudioOMS.Authorization;

internal class AuthorizationClient : IStudioOMSClient
{
    public IUserClient User => throw new NotImplementedException();
    public IEmployeeClient Employee => throw new NotImplementedException();
    public ICustomerClient Customer => throw new NotImplementedException();
    public IOrderClient Order => throw new NotImplementedException();

    public ValueTask<LoginResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}