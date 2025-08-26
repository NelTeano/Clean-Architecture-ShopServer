using AutoMapper;
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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateVariantCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<VariantResponseDto> Handle(CreateVariantCommand request, CancellationToken token)
        {
            // DTO -> Entity
            var entity = _mapper.Map<VariantEntity>(request.Dto);

            await _uow.Variant.Add(entity, token);
            await _uow.CommitAsync(token);

            // Entity -> Response DTO
            return _mapper.Map<VariantResponseDto>(entity);
        }
    }
}
