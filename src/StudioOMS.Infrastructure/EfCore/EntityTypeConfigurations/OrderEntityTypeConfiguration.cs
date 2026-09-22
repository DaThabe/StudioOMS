using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioOMS.Customers;
using StudioOMS.EfCore.ValueComparers;
using StudioOMS.EfCore.ValueConverters;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

namespace StudioOMS.EfCore.EntityTypeConfigurations;


internal sealed class OrderEntityTypeConfiguration :
    IEntityTypeConfiguration<Order>,
    IEntityTypeConfiguration<TimingOrder>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");

        // Id
        builder.HasKey(x => x.Id);
        // CustomerId
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        // SalespersonId
        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.SalespersonId)
            .OnDelete(DeleteBehavior.Restrict);


        // Title
        builder.Property(x => x.Title)
            .IsRequired();
        // State
        builder.Property(x => x.State)
            .HasConversion<string>();
        // CreateAt
        builder.Property(x => x.CreateAt)
            .IsRequired();

        // AssignedEmployees
        builder.Property(x => x.AssignedEmployees)
            .HasField("_assignedEmployees")
            .HasConversion<HashSetEmployeeId_String_Converter>()
            .Metadata.SetValueComparer(new HashSetEmployeeId_String_Comparer());

        // StateTransitions
        builder.Property(x => x.StateTransitions)
            .HasField("_stateTransitions")
            .HasConversion<SortedSetOrderStateTransition_String_Converter>()
            .Metadata.SetValueComparer(new SortedSetOrderStateTransition_String_Comparer());

        // 派生类
        builder.HasDiscriminator<OrderType>("Type")
            .HasValue<TimingOrder>(OrderType.Timing);
    }

    public void Configure(EntityTypeBuilder<TimingOrder> builder)
    {
        builder.HasMany(x => x.Consumes)
           .WithOne()
           .HasForeignKey("OrderId")
           .OnDelete(DeleteBehavior.Cascade);
    }
}
