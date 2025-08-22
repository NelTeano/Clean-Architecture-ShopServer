namespace MyServer.Application.Models.DTOs.CommandsDTO.ProductSizes
{

    public class CreateProductSizeDTO
    {
        public string Size { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
