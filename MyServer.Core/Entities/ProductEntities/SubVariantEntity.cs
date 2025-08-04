using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyServer.Core.Entities.ProductEntities
{
    public class SubVariantEntity
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        // Foreign key
        public int VariantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("VariantId")]
        public virtual VariantEntity Variant { get; set; } = null!;
        public virtual ICollection<CategoryEntity> Categories { get; set; } = new List<CategoryEntity>();
    }
}
