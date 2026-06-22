using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("product_images")]
[Index("ProductId", Name = "idx_product_images_product")]
public partial class ProductImage
{
    [Key]
    [Column("image_id")]
    public Guid ImageId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("image_url")]
    public string ImageUrl { get; set; } = null!;

    [Column("display_order")]
    public int DisplayOrder { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductImages")]
    public virtual Product Product { get; set; } = null!;
}
