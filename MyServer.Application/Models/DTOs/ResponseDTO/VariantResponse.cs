using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Models.DTOs.ResponseDTO
{
    public class VariantResponseDto
    {
        public int Id { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public List<SubVariantResponseDto> SubVariants { get; set; } = new();
    }

    public class SubVariantResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryResponseDto> Categories { get; set; } = new();
    }

    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductItemResponseDto> Items { get; set; } = new();
    }

    public class ProductItemResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public List<ProductSizeResponseDto> Sizes { get; set; } = new();
    }

    public class ProductSizeResponseDto
    {
        public int Id { get; set; }
        public string Size { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
