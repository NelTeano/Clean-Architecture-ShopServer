using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Models.DTOs
{
    public class VariantDto
    {
        public int Id { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class VariantWithSubVariantsDto
    {
        public int Id { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<SubVariantDto> SubVariants { get; set; } = new();
    }

    public class SubVariantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int VariantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
