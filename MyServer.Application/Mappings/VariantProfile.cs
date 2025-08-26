using AutoMapper;
using MyServer.Application.Models.DTOs;
using MyServer.Application.Models.DTOs.CommandsDTO.Category;
using MyServer.Application.Models.DTOs.CommandsDTO.ProductItem;
using MyServer.Application.Models.DTOs.CommandsDTO.ProductSizes;
using MyServer.Application.Models.DTOs.CommandsDTO.SubVariant;
using MyServer.Application.Models.DTOs.CommandsDTO.Variant;
using MyServer.Application.Models.DTOs.ResponseDTO;
using MyServer.Core.Entities.ProductEntities;

namespace MyServer.Application.Mappings
{
    public class VariantProfile : Profile
    {
        public VariantProfile()
        {
            // Command DTO -> Entity
            CreateMap<CreateVariantDTO, VariantEntity>();
            CreateMap<CreateSubVariantDTO, SubVariantEntity>();
            CreateMap<CreateCategoryDTO, CategoryEntity>();
            CreateMap<CreateProductItemDTO, ProductItemEntity>();
            CreateMap<CreateProductSizeDTO, ProductSizeEntity>();

            // Entity -> Response DTO
            CreateMap<VariantEntity, VariantResponseDto>();
            CreateMap<SubVariantEntity, SubVariantResponseDto>();
            CreateMap<CategoryEntity, CategoryResponseDto>();
            CreateMap<ProductItemEntity, ProductItemResponseDto>();
            CreateMap<ProductSizeEntity, ProductSizeResponseDto>();
        }
    }
}
