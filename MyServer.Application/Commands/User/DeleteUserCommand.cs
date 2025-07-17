using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.User
{
    public record DeleteUserCommand(Guid UserId) : IRequest<UserEntity>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserEntity>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEntity> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteUser(request.UserId, cancellationToken);
        }
    }
}
