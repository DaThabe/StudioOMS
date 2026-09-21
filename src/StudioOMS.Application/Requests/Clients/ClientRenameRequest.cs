using StudioOMS.Clients;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Clients;


public sealed class ClientRenameRequest : IRequest
{
    public required ClientId Id { get; init; }
    public required string Name { get; init; }


    public static ClientRenameRequest FromDto(Guid clientId, ClientRenameDto dto)
    {
        return new()
        {
            Id = new(clientId),
            Name = dto.Name
        };
    }

    internal sealed class Handler(
            IClientRepository clientRepository
        ) : IRequestHandler<ClientRenameRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.ClientManage);

        public async ValueTask HandleAsync(ClientRenameRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = await clientRepository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"客户 [{request.Id}] 不存在");

            entity.Rename(request.Name);
            await clientRepository.SaveAsync(entity, cancellationToken);
        }
    }
}