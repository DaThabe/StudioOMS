using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioOMS.EfCore.ValueConverters;
using StudioOMS.Employees;

namespace StudioOMS.EfCore.EntityTypeConfigurations;

internal sealed class EmployeeEntityTypeConfiguration :
    IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        // Id
        builder.HasKey(x => x.Id);
        // Name
        builder.Property(x => x.Name)
            .IsRequired();
        builder.Property(x => x.Roles)
           .HasField("_roles")
           .HasConversion<IReadonlySetEmployeeRole_String_Converter>();
    }
}
