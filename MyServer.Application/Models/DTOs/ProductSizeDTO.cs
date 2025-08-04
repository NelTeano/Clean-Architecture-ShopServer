using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Models.DTOs
{
    public class ProductSizeDTO
    {
        public int? Id { get; set; }
        public string Size { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }


    public class CreateProductSizeRequest
    {
        [Required]
        public string Size { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }

    public class UpdateProductSizeRequest
    {
        public int Id { get; set; }

        [Required]
        public string Size { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
