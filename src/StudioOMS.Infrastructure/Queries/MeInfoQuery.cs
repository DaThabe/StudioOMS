using Microsoft.EntityFrameworkCore;
using StudioOMS.EfCore;
using StudioOMS.Me;
using StudioOMS.Users;

namespace StudioOMS.Queries;


internal sealed class MineInfoQuery(AppDbContext appDbContext) : IMineInfoQuery
{
    public async ValueTask<MineInfoResponse?> QueryAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var infoResponsesQuery =
            from user in appDbContext.Users.AsNoTracking()
            where user.Id == id
            join employee in appDbContext.Employees on user.EmployeeId equals employee.Id
            select new MineInfoResponse()
            {
                Id = user.Id,
                Username = user.Username,
                EmployeeId = employee.Id,
                EmployeeName = employee.Name
            };

        return await infoResponsesQuery.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
