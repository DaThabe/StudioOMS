using StudioOMS.Messaging;
using StudioOMS.Users;

namespace StudioOMS.Clients;


public sealed class ClientCreateRequest : IRequest
{
    public required ClientId Id { get; init; }
    public required string Name { get; init; }


    public static implicit operator ClientCreateRequest(ClientCreateDto dto)
    {
        return new()
        {
            Id = new ClientId(dto.Id),
            Name = dto.Name,
        };
    }

    internal sealed class Handler(IClientRepository clientRepository) : IRequestHandler<ClientCreateRequest>
    {
        public async ValueTask HandleAsync(ClientCreateRequest request, CancellationToken cancellationToken = default)
        {
            var user = Client.Create(request.Id);
            user.Rename(request.Name);

            await clientRepository.SaveAsync(user, cancellationToken);
        }
    }
}