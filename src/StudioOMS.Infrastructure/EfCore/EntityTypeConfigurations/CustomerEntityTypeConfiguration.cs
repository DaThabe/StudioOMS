using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioOMS.Customers;

namespace StudioOMS.EfCore.EntityTypeConfigurations;

internal sealed class CustomerEntityTypeConfiguration :
    IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        // Id
        builder.HasKey(x => x.Id);
        // Name
        builder.Property(x => x.Name)
            .IsRequired();
    }
}
