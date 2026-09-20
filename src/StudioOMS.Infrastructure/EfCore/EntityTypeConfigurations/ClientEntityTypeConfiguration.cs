using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioOMS.Clients;

namespace StudioOMS.EfCore.EntityTypeConfigurations;

internal sealed class ClientEntityTypeConfiguration :
    IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Client");

        // Id
        builder.HasKey(x => x.Id);
        // Name
        builder.Property(x => x.Name)
            .IsRequired();
    }
}
