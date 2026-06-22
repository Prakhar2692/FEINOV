using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("products")]
[Index("ProductName", Name = "idx_products_name")]
[Index("SubcategoryId", Name = "idx_products_subcategory")]
public partial class Product
{
    [Key]
    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("subcategory_id")]
    public Guid SubcategoryId { get; set; }

    [Column("product_name")]
    [StringLength(300)]
    public string ProductName { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("brand")]
    [StringLength(200)]
    public string? Brand { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("updated_date", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedDate { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    [ForeignKey("SubcategoryId")]
    [InverseProperty("Products")]
    public virtual Subcategory Subcategory { get; set; } = null!;
}
