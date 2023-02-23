using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using productstockingv1.models;

namespace productstockingv1.EntityConfiguration;

public class StockingConfig : IEntityTypeConfiguration<Stocking>
{
    public void Configure(EntityTypeBuilder<Stocking> builder)
    {
        builder.ToTable("stocking").HasKey(e => e.Id).HasName("PRIMARY");
        builder.HasIndex(e => e.ProductId, "stk_pro_fk");
        builder.HasIndex(e => e.WareId, "stk_wre_fk");

        builder.Property(e => e.Id).IsRequired().HasColumnType("varchar").HasMaxLength(36);
        builder.Property(e => e.ProductId).IsRequired().HasColumnType("varchar").HasMaxLength(36);
        builder.Property(e => e.WareId).IsRequired().HasColumnType("varchar").HasMaxLength(36);
        builder.Property(e => e.Quantity).IsRequired(false).HasColumnType("int");
        builder.Property(e => e.DocumentDate).IsRequired(false).HasColumnType("datetime");
        builder.Property(e => e.PostingDate).IsRequired(false).HasColumnType("datetime");

        builder.HasOne(st => st.product)
            .WithMany(p => p.Stockings)
            .HasForeignKey(st => st.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("stk_pro_fk");
        builder.HasOne(st => st.Ware)
            .WithMany(w => w.Stockings)
            .HasForeignKey(st => st.WareId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("stk_wre_fk");
    }
}