using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using productstockingv1.models;

namespace productstockingv1.EntityConfiguration;

public class WareConfig : IEntityTypeConfiguration<Ware>
{
    public void Configure(EntityTypeBuilder<Ware> builder)
    {
        builder.ToTable("ware").HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();

        builder.Property(e => e.Id).IsRequired().HasColumnType("varchar").HasMaxLength(36);
        builder.Property(e => e.Code).IsRequired().HasColumnType("varchar").HasMaxLength(36);
        builder.Property(e => e.Name).IsRequired().HasColumnType("varchar").HasMaxLength(50);
        builder.Property(e => e.Description).IsRequired(false).HasColumnType("varchar").HasMaxLength(200);
        
    }
}