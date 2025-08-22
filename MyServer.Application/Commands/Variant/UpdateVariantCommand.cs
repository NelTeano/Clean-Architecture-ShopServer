using MediatR;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.Variant
{
    public record UpdateVariantCommand(VariantEntity variant) : IRequest<VariantEntity>;

    public class UpdateVariantCommandHandler : IRequestHandler<UpdateVariantCommand, VariantEntity>
    {
        private readonly IVariantRepository _variantRepository;

        public UpdateVariantCommandHandler(IVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }

        public async Task<VariantEntity> Handle(UpdateVariantCommand request, CancellationToken token)
        {
            var variant = await _variantRepository.GetById(request.variant.Id, token);
            if (variant == null) return null;

            variant.VariantName = request.variant.VariantName;
            variant.UpdatedAt = DateTime.UtcNow;

            var updatedVariant = await _variantRepository.Update(variant.Id, variant, token);
            return updatedVariant;
        }
    }
    
}
