using MyServer.Application.Models.DTOs.CommandsDTO.Category;

namespace MyServer.Application.Models.DTOs.CommandsDTO.SubVariant
{
    public class CreateSubVariantDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<CreateCategoryDTO> Categories { get; set; } = new();
    }

}
