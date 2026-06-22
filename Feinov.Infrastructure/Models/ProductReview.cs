using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("product_reviews")]
public partial class ProductReview
{
    [Key]
    [Column("review_id")]
    public Guid ReviewId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("rating")]
    public int Rating { get; set; }

    [Column("review_title")]
    [StringLength(200)]
    public string? ReviewTitle { get; set; }

    [Column("review_text")]
    public string? ReviewText { get; set; }

    [Column("is_approved")]
    public bool IsApproved { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }
}
