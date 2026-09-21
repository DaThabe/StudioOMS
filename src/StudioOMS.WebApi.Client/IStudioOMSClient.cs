using StudioOMS.Clients;
using StudioOMS.Http;
using StudioOMS.Routes;
using System.Net.Http.Headers;

namespace StudioOMS;


public interface IStudioOMSClient
{
    IMeClient Me { get; }
    IUserClient User { get; }
    IEmployeeClient Employee { get; }
    ICustomerClient Customer { get; }
    IOrderClient Order { get; }
}


internal sealed class StudioOMSClient : IStudioOMSClient
{
    public IMeClient Me { get; }
    public IUserClient User { get; }
    public IEmployeeClient Employee { get; }
    public ICustomerClient Customer { get; }
    public IOrderClient Order { get; }



    public StudioOMSClient(HttpClient httpClient, ServerRoutes routes, AuthenticationHeaderValue authentication)
    {
        Me = new MeClient(routes.Me, httpClient, OptionRequest);
        User = new UserClient(routes.User, httpClient, OptionRequest);
        Employee = new EmployeeClient(routes.Employee, httpClient, OptionRequest);
        Customer = new CustomerClient(routes.Customer, httpClient, OptionRequest);
        Order = new OrderClient(routes.Order, httpClient, OptionRequest);

        void OptionRequest(HttpRequestMessage message) =>
             message.Headers.Authorization = authentication;
    }

    public static StudioOMSClient Create(HttpClient httpClient, AuthenticationHeaderValue authentication)
    {
        var routes = httpClient.GetServerRoutes();
        return new(httpClient, routes, authentication);
    }
}