using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Entities.ProductEntities
{
    public class ProductItemEntity
    {

        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Image { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; } = 0;

        // Foreign key
        public int CategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual CategoryEntity Category { get; set; } = null!;
        public virtual ICollection<ProductSizeEntity> Sizes { get; set; } = new List<ProductSizeEntity>();
    }
}
