using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Entities.ProductEntities
{
    public class CategoryEntity
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        // Foreign key
        public int SubVariantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("SubVariantId")]
        public virtual SubVariantEntity SubVariant { get; set; } = null!;
        public virtual ICollection<ProductItemEntity> Items { get; set; } = new List<ProductItemEntity>();
    }
}
