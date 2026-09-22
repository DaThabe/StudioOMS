using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Session;
using StudioOMS.Users;

namespace StudioOMS.Me;


public sealed class MineInfoRequest : IRequest<MeInfoResponse?>
{
    internal sealed class Handler(
            ICurrentSession currentSession,
            IMineInfoQuery infoQuery
        ) : IRequestHandler<MineInfoRequest, MeInfoResponse?>, IAuthentication
    {
        public async ValueTask<MeInfoResponse?> HandleAsync(MineInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            return await infoQuery.QueryAsync(currentSession.UserId, cancellationToken);
        }
    }
}

public interface IMineInfoQuery
{
    ValueTask<MeInfoResponse?> QueryAsync(UserId id, CancellationToken cancellationToken = default);
}

public sealed record class MeInfoResponse
{
    public required UserId Id { get; init; }
    public required Username Username { get; init; }
    public required EmployeeId EmployeeId { get; init; }
    public required EmployeeName EmployeeName { get; init; }
}