using StudioOMS.Clients;
using StudioOMS.Extensions;
using StudioOMS.Login;
using StudioOMS.Routes;
using StudioOMS.Serializer;
using System.Net.Http.Headers;

namespace StudioOMS;


public interface IStudioOMSClient
{
    IUserClient User { get; }
    IEmployeeClient Employee { get; }
    ICustomerClient Customer { get; }
    IOrderClient Order { get; }
}


internal sealed class StudioOMSClient : IStudioOMSClient
{
    private readonly static HttpClient _sharedHttpClient = new();


    public IMeClient Me { get; }
    public IUserClient User { get; }
    public IEmployeeClient Employee { get; }
    public ICustomerClient Customer { get; }
    public IOrderClient Order { get; }



    public StudioOMSClient(ServerRoutes routes, AuthenticationHeaderValue authentication)
    {
        Me = new MeClient(routes.Me, _sharedHttpClient, OptionRequest);
        User = new UserClient(routes.User, _sharedHttpClient, OptionRequest);
        Employee = new EmployeeClient(routes.Employee, _sharedHttpClient, OptionRequest);
        Customer = new CustomerClient(routes.Customer, _sharedHttpClient, OptionRequest);
        Order = new OrderClient(routes.Order, _sharedHttpClient, OptionRequest);

        void OptionRequest(HttpRequestMessage message) =>
             message.Headers.Authorization = authentication;
    }
    public StudioOMSClient(Uri baseUrl, AuthenticationHeaderValue authentication) : this(new ServerRoutes(baseUrl), authentication)
    {

    }
    public StudioOMSClient(string baseUrl, AuthenticationHeaderValue authentication) : this(new Uri(baseUrl), authentication)
    {

    }


    public static async Task<StudioOMSClient> LoginAsync(ServerRoutes routes, LoginDto dto, CancellationToken cancellationToken = default)
    {
        var request = HttpRequestMessage.PostJson(routes.Login, dto, AppJsonSerializerContext.Default.LoginDto);
        var loginResult = await _sharedHttpClient.GetJsonAsync(request, AppJsonSerializerContext.Default.LoginResult, cancellationToken);

        return new StudioOMSClient(routes, new("Bearer", loginResult.Token));
    }

    public static Task<StudioOMSClient> LoginAsync(Uri baseUrl, LoginDto dto, CancellationToken cancellationToken = default) =>
        LoginAsync(new ServerRoutes(baseUrl), dto, cancellationToken);

    public static Task<StudioOMSClient> LoginAsync(string baseUrl, LoginDto dto, CancellationToken cancellationToken = default) =>
        LoginAsync(new Uri(baseUrl), dto, cancellationToken);
}