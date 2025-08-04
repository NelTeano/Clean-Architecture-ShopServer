using System.ComponentModel.DataAnnotations;

namespace MyServer.Application.Models.DTOs
{
    public class CategoryDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductItemDTO>? Items { get; set; }
    }

    public class CreateCategoryRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public List<CreateProductItemRequest>? Items { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
    }

}
