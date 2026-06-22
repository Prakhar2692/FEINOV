using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("variant_discounts")]
public partial class VariantDiscount
{
    [Key]
    [Column("discount_id")]
    public Guid DiscountId { get; set; }

    [Column("variant_id")]
    public Guid VariantId { get; set; }

    [Column("discount_percentage")]
    [Precision(5, 2)]
    public decimal? DiscountPercentage { get; set; }

    [Column("start_date", TypeName = "timestamp without time zone")]
    public DateTime? StartDate { get; set; }

    [Column("end_date", TypeName = "timestamp without time zone")]
    public DateTime? EndDate { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }
}
