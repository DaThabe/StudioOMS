using StudioOMS.Clients;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Clients;


public sealed class ClientCreateRequest : IRequest<ClientId>
{
    public required string Name { get; init; }


    public static ClientCreateRequest FromDto(ClientCreateDto dto)
    {
        return new() { Name = dto.Name };
    }


    internal sealed class Handler(
            IClientRepository clientRepository
        ) : IRequestHandler<ClientCreateRequest, ClientId>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.ClientCreate);

        public async ValueTask<ClientId> HandleAsync(ClientCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var client = Client.Create();
            client.Rename(request.Name);
            await clientRepository.SaveAsync(client, cancellationToken);

            return client.Id;
        }
    }
}