using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Id)
            .HasColumnName("category_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(category => category.CategoryName)
            .HasColumnName("category_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(category => category.Description)
            .HasColumnName("description");

        builder.Property(category => category.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(category => category.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(category => category.UpdatedDate)
            .HasColumnName("updated_date");

        builder.HasIndex(category => category.CategoryName)
            .IsUnique()
            .HasDatabaseName("idx_categories_name");
    }
}
