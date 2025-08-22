using MediatR;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.Variant
{
    public record RemoveVariantCommand(VariantEntity variant) : IRequest<VariantEntity>;

    public class RemoveVariantCommandHandler : IRequestHandler<RemoveVariantCommand, VariantEntity>
    {
        private readonly IVariantRepository _variantRepository;
        public RemoveVariantCommandHandler(IVariantRepository variantRepository) {
            _variantRepository = variantRepository;
        }

        public async Task<VariantEntity> Handle(RemoveVariantCommand request, CancellationToken token)
        {
            var getVariant = await _variantRepository.GetById(request.variant.Id, token);

            if (getVariant == null)
                return null;

            var removeVariant = await _variantRepository.Remove(request.variant.Id, token);
            return removeVariant;
        }

    }


}
