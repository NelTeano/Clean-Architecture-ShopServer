using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Models.DTOs
{
    public class VariantDTO
    {
        public int? Id { get; set; }
        public string Variant { get; set; } = string.Empty;
        public List<SubVariantDTO>? SubVariants { get; set; }
    }

    public class CreateVariantRequest
    {
        [Required]
        [StringLength(255)]
        public string Variant { get; set; } = string.Empty;

        public List<CreateSubVariantRequest>? SubVariants { get; set; }
    }

    public class UpdateVariantRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Variant { get; set; } = string.Empty;
    }
}
