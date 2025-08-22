using MyServer.Application.Models.DTOs.CommandsDTO.ProductSizes;

namespace MyServer.Application.Models.DTOs.CommandsDTO.ProductItem
{
    public class CreateProductItemDTO
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; } = 0;
        public List<CreateProductSizeDTO> Sizes { get; set; } = new();
    }
}
