using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("wishlists")]
public partial class Wishlist
{
    [Key]
    [Column("wishlist_id")]
    public Guid WishlistId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }
}
