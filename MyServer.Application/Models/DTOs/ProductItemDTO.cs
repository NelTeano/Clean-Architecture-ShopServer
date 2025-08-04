using System.ComponentModel.DataAnnotations;

namespace MyServer.Application.Models.DTOs
{
    public class ProductItemDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public List<ProductSizeDTO>? Sizes { get; set; }
    }


    public class CreateProductItemRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Image { get; set; }

        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        public List<CreateProductSizeRequest>? Sizes { get; set; }
    }

    public class UpdateProductItemRequest
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Image { get; set; }

        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;
    }
}
