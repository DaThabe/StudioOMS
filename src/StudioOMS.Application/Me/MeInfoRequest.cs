using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Session;
using StudioOMS.Users;

namespace StudioOMS.Me;


public sealed class MeInfoRequest : IRequest<MeInfoResponse>
{
    internal sealed class Handler(
            ICurrentSession currentSession,
            IMeInfoQuery infoQuery
        ) : IRequestHandler<MeInfoRequest, MeInfoResponse>, IAuthentication
    {
        public async ValueTask<MeInfoResponse> HandleAsync(MeInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            return await infoQuery.QueryAsync(currentSession.UserId, cancellationToken) ??
                throw new InvalidOperationException();
        }
    }
}

public interface IMeInfoQuery
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