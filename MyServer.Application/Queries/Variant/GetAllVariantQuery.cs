using MediatR;
using MyServer.Application.Models.DTOs;
namespace MyServer.Application.Queries.Variant
{
    //public class GetVariantByIdQuery : IRequest<VariantDto?>
    //{
    //    public int Id { get; set; }
    //}

    //public class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, VariantDto?>
    //{
    //    private readonly ApplicationContextDB _context;

    //    public GetVariantByIdQueryHandler(ApplicationContextDB context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<VariantDto?> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    //    {
    //        var variant = await _context.Variants
    //            .Where(v => v.Id == request.Id)
    //            .Select(v => new VariantDto
    //            {
    //                Id = v.Id,
    //                VariantName = v.VariantName,
    //                CreatedAt = v.CreatedAt,
    //                UpdatedAt = v.UpdatedAt
    //            })
    //            .FirstOrDefaultAsync(cancellationToken);

    //        return variant;
    //    }
    //}
}
