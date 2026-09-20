using StudioOMS.Endpoints.Clients;
using StudioOMS.Endpoints.Employees;
using StudioOMS.Endpoints.Orders;
using StudioOMS.Endpoints.Users;

namespace StudioOMS.Endpoints;

public static class EndpointExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapStudioOMSEndpoints()
        {
            return app
                .MapUserEndpoints()
                .MapEmployeeEndpoints()
                .MapClientEndpoints()
                .MapOrderEndpoints();
        }
    }
}
