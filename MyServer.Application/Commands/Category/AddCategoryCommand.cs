using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.Category
{
    public record AddCategoryCommand(CategoryEntity Category) : IRequest<CategoryEntity>;

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryEntity>
    {
        private readonly ICategoryRepository _categoryRepository;

        public AddCategoryCommandHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

        public async Task<CategoryEntity> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            // Fix missing semicolon
            return await _categoryRepository.AddAsync(request.Category, cancellationToken);
        }
    }
}
