using MediatR;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.Category
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;
        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
        {
            private readonly ICategoryRepository _repository;
            public DeleteCategoryCommandHandler(ICategoryRepository repository)
            {
                _repository = repository;
            }
            public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                return await _repository.DeleteAsync(request.Id, cancellationToken);
            }
        }
}
