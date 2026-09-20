using Microsoft.EntityFrameworkCore;
using StudioOMS.EfCore;
using StudioOMS.Employees;

namespace StudioOMS.Repositories;

internal sealed class EmployeeRepository(AppDbContext appDbContext) : Repository<Employee, EmployeeId>, IEmployeeRepository
{
    protected override AppDbContext DbContext => appDbContext;
    protected override DbSet<Employee> Entities => appDbContext.Employees;

    public ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        new(appDbContext.Employees.AnyAsync(cancellationToken: cancellationToken));

    public ValueTask<IReadOnlyList<Employee>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default) =>
        GetAllOrderedAsync(x => x.Id, skip, take, cancellationToken);
}