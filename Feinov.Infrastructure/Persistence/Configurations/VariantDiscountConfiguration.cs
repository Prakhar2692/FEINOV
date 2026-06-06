using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class VariantDiscountConfiguration : IEntityTypeConfiguration<VariantDiscount>
{
    public void Configure(EntityTypeBuilder<VariantDiscount> builder)
    {
        builder.ToTable("variant_discounts");

        builder.HasKey(discount => discount.Id);

        builder.Property(discount => discount.Id)
            .HasColumnName("discount_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(discount => discount.VariantId)
            .HasColumnName("variant_id");

        builder.Property(discount => discount.DiscountPercentage)
            .HasColumnName("discount_percentage")
            .HasColumnType("decimal(5,2)");

        builder.Property(discount => discount.StartDate)
            .HasColumnName("start_date");

        builder.Property(discount => discount.EndDate)
            .HasColumnName("end_date");

        builder.Property(discount => discount.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasOne(discount => discount.Variant)
            .WithMany(variant => variant.Discounts)
            .HasForeignKey(discount => discount.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
