using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }

    DbSet<Subcategory> Subcategories { get; }

    DbSet<Product> Products { get; }

    DbSet<ProductImage> ProductImages { get; }

    DbSet<ProductVariant> ProductVariants { get; }

    DbSet<VariantDiscount> VariantDiscounts { get; }

    DbSet<Inventory> Inventories { get; }

    DbSet<User> Users { get; }

    DbSet<CustomerAddress> CustomerAddresses { get; }

    DbSet<Cart> Carts { get; }

    DbSet<Order> Orders { get; }

    DbSet<OrderItem> OrderItems { get; }

    DbSet<OrderAddress> OrderAddresses { get; }

    DbSet<PaymentTransaction> PaymentTransactions { get; }

    DbSet<OtpTransaction> OtpTransactions { get; }

    DbSet<ProductReview> ProductReviews { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
