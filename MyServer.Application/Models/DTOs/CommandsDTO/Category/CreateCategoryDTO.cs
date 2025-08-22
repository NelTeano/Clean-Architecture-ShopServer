using MyServer.Application.Models.DTOs.CommandsDTO.ProductItem;

namespace MyServer.Application.Models.DTOs.CommandsDTO.Category
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<CreateProductItemDTO> Items { get; set; } = new();
    }
}
