using MyServer.Application.Models.DTOs.CommandsDTO.SubVariant;

namespace MyServer.Application.Models.DTOs.CommandsDTO.Variant
{
    public class CreateVariantDTO
    {
        public string VariantName { get; set; } = string.Empty;
        public List<CreateSubVariantDTO> SubVariants { get; set; } = [];
    }
}
