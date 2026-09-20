using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StudioOMS.Clients;
using StudioOMS.EfCore.EntityTypeConfigurations;
using StudioOMS.EfCore.ValueConverters;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Users;
using System.Diagnostics.CodeAnalysis;

namespace StudioOMS.EfCore;


[RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
[RequiresDynamicCode("EF Core isn't fully compatible with NativeAOT, and running the application may generate unexpected runtime failures.")]
internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; init; }
    public DbSet<TimingOrder> TimingOrders { get; init; }

    public DbSet<OrderConsume> Consumes { get; init; }
    public DbSet<TimingOrderConsume> TimingConsumes { get; init; }

    public DbSet<Client> Clients { get; init; }
    public DbSet<Employee> Employees { get; init; }
    public DbSet<User> Users { get; init; }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // 用户
        configurationBuilder.Properties<UserId>()
            .HaveConversion<UserId_String_Converter>();
        // 员工
        configurationBuilder.Properties<EmployeeId>()
            .HaveConversion<EmployeeId_String_Converter>();
        // 客户
        configurationBuilder.Properties<ClientId>()
            .HaveConversion<ClientId_String_Converter>();
        // 订单
        configurationBuilder.Properties<OrderId>()
            .HaveConversion<OrderId_String_Converter>();
        // 划扣
        configurationBuilder.Properties<ConsumeId>()
            .HaveConversion<ConsumeId_String_Converter>();

        // 时间
        configurationBuilder.Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffset_String_Converter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityTypeConfiguration).Assembly);
    }
}


[RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
[RequiresDynamicCode("EF Core isn't fully compatible with NativeAOT, and running the application may generate unexpected runtime failures.")]
internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=studio_oms.db")
            .Options;

        return new AppDbContext(options);
    }
}