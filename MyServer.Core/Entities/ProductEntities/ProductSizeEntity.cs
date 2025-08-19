

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyServer.Core.Entities.ProductEntities
{
    public class ProductSizeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Size { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Foreign key
        public int ProductItemId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        [ForeignKey("ProductItemId")]
        public virtual ProductItemEntity ProductItem { get; set; } = null!;
    }
}
