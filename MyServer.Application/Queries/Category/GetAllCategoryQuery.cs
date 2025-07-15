using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Queries.Category
{
    public record GetAllCategoriesQuery() : IRequest<IEnumerable<CategoryEntity>>;

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryEntity>>
    {
        private readonly ICategoryRepository _repository;

        public GetAllCategoriesQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryEntity>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}
