using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Models.DTOs
{
    public class SubVariantDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryDTO>? Categories { get; set; }
    }

    public class CreateSubVariantRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public List<CreateCategoryRequest>? Categories { get; set; }
    }

    public class UpdateSubVariantRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
    }
}
