using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.EfCore.EntityTypeConfigurations;

internal sealed class UserEntityTypeConfiguration :
    IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        // Id
        builder.HasKey(x => x.Id);
        // EmployeeId
        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Name
        builder.Property(x => x.Name)
            .IsRequired();
        // Password
        builder.Property(x => x.PasswordHash)
            .IsRequired();
    }
}