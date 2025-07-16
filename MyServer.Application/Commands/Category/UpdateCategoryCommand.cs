using MediatR;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.Category
{

    public record UpdateCategoryCommand(Guid Id, string Name, string? Description, bool IsActive) : IRequest<bool>;
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _repository;
        public UpdateCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
            {
                return false; // Category not found
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                category.Name = request.Name;
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                category.Description = request.Description;
            }

            var updatedCategory = await _repository.UpdateAsync(request.Id, category, cancellationToken);
            return true;
        }
    }
}
