using System.ComponentModel.DataAnnotations;

namespace MyServer.Core.Entities.ProductEntities
{
    public class VariantEntity
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string VariantName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        // Navigation property
        public virtual ICollection<SubVariantEntity> SubVariants { get; set; } = new List<SubVariantEntity>();
    }
}
