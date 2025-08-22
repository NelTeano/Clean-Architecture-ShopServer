using MediatR;
using MyServer.Application.Models.DTOs.CommandsDTO.Variant;
using MyServer.Application.Models.DTOs.ResponseDTO;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.Variant
{
    public record CreateVariantCommand(CreateVariantDTO Dto) : IRequest<VariantResponseDto>;

    public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, VariantResponseDto>
    {
        private readonly IVariantRepository _variantRepository;

        public CreateVariantCommandHandler(IVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }

        public async Task<VariantResponseDto> Handle(CreateVariantCommand request, CancellationToken token)
        {
            // Map DTO -> Entity
            var entity = new VariantEntity
            {
                VariantName = request.Dto.VariantName,
                SubVariants = request.Dto.SubVariants.Select(sv => new SubVariantEntity
                {
                    Name = sv.Name,
                    Categories = sv.Categories.Select(c => new CategoryEntity
                    {
                        Name = c.Name,
                        Items = c.Items.Select(i => new ProductItemEntity
                        {
                            Name = i.Name,
                            Image = i.Image,
                            Description = i.Description,
                            Quantity = i.Quantity,
                            Sizes = i.Sizes.Select(s => new ProductSizeEntity
                            {
                                Size = s.Size,
                                Price = s.Price
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            var saved = await _variantRepository.Add(entity, token);

            // Map Entity -> Response DTO
            return new VariantResponseDto
            {
                Id = saved.Id,
                VariantName = saved.VariantName,
                SubVariants = saved.SubVariants.Select(sv => new SubVariantResponseDto
                {
                    Id = sv.Id,
                    Name = sv.Name,
                    Categories = sv.Categories.Select(c => new CategoryResponseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Items = c.Items.Select(i => new ProductItemResponseDto
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Image = i.Image,
                            Description = i.Description,
                            Quantity = i.Quantity,
                            Sizes = i.Sizes.Select(s => new ProductSizeResponseDto
                            {
                                Id = s.Id,
                                Size = s.Size,
                                Price = s.Price
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }
    }
}
