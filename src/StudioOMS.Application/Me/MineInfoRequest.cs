using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Me;


public sealed class MineInfoRequest : IRequest<MineInfoResponse>
{
    internal sealed class Handler(
            ICurrentSession currentSession,
            IMineInfoQuery infoQuery
        ) : IRequestHandler<MineInfoRequest, MineInfoResponse>, IAuthentication
    {
        public async ValueTask<MineInfoResponse?> HandleAsync(MineInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            if (currentSession.Info is null)
                throw new InvalidOperationException("未登录");

            return await infoQuery.QueryAsync(currentSession.Info.UserId, cancellationToken);
        }
    }
}

public interface IMineInfoQuery
{
    ValueTask<MineInfoResponse?> QueryAsync(UserId id, CancellationToken cancellationToken = default);
}

public sealed record class MineInfoResponse
{
    public required UserId Id { get; init; }
    public required Username Username { get; init; }
    public required EmployeeId EmployeeId { get; init; }
    public required EmployeeName EmployeeName { get; init; }
}