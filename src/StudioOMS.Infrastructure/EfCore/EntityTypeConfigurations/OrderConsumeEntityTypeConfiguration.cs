using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS.EfCore.EntityTypeConfigurations;


internal sealed class OrderConsumeEntityTypeConfiguration :
    IEntityTypeConfiguration<OrderConsume>,
    IEntityTypeConfiguration<TimingOrderConsume>
{
    public void Configure(EntityTypeBuilder<OrderConsume> builder)
    {
        builder.ToTable("Order.Consume");

        // Id
        builder.HasKey(x => x.Id);
        // OrderId
        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);
        // EmployeeId
        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Time
        builder.Property(x => x.Timestamp)
            .IsRequired();
    }

    public void Configure(EntityTypeBuilder<TimingOrderConsume> builder)
    {
        // Days
        builder.Property(x => x.Days)
            .IsRequired();
    }
}
