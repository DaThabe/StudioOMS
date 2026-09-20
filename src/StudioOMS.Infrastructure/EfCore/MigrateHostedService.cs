using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace StudioOMS.EfCore;


[RequiresDynamicCode("Migrations operations are not supported with NativeAOT Use a migration bundle or an alternate way of executing migration operations.")]
internal sealed class MigrateHostedService(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        appDbContext.Database.EnsureCreated();
        //await appDbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
