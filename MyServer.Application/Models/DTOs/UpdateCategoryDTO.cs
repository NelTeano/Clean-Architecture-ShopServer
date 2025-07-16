using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Models.DTOs
{
    public class UpdateCategoryDTO
    {
        public string Name { get; set; } = string.Empty; 
        public string? Description { get; set; } 
        public bool IsActive { get; set; } = true; 
    }
}