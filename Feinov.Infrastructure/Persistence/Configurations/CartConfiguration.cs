using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");

        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.Id)
            .HasColumnName("cart_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(cart => cart.UserId)
            .HasColumnName("user_id");

        builder.Property(cart => cart.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(cart => cart.User)
            .WithMany(user => user.Carts)
            .HasForeignKey(cart => cart.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_cart_user");
    }
}
