using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("subcategories")]
[Index("CategoryId", Name = "idx_subcategories_category")]
[Index("CategoryId", "SubcategoryName", Name = "idx_subcategory_name_per_category", IsUnique = true)]
public partial class Subcategory
{
    [Key]
    [Column("subcategory_id")]
    public Guid SubcategoryId { get; set; }

    [Column("category_id")]
    public Guid CategoryId { get; set; }

    [Column("subcategory_name")]
    [StringLength(200)]
    public string SubcategoryName { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("updated_date", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Subcategories")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Subcategory")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
