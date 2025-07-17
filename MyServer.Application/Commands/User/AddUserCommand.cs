using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.User
{
    public record AddUserCommand(UserEntity User) : IRequest<UserEntity>;

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserEntity>
    {
        private readonly IUserRepository _userRepository;
        public AddUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEntity> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.AddUser(request.User, cancellationToken);
        }
    }
}
