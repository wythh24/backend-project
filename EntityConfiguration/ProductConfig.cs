using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using productstockingv1.models;

namespace productstockingv1.EntityConfiguration;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product").HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();

        builder.Property(e => e.Id).IsRequired().HasColumnType("varchar").HasMaxLength(36);
        builder.Property(e => e.Code).IsRequired().HasColumnType("varchar").HasMaxLength(36);
        builder.Property(e => e.Name).IsRequired().HasColumnType("varchar").HasMaxLength(50);
        builder.Property(e => e.Price).IsRequired().HasColumnType("dec(10,2)");
        builder.Property(e => e.Description).IsRequired(false).HasColumnType("varchar").HasMaxLength(200).IsUnicode(true);
    }
}