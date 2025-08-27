using MediatR;
using Microsoft.Extensions.Logging;
using MyServer.Application.Commands.Variant;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Commands.User
{
    public record AddUserCommand(UserEntity User) : IRequest<UserEntity>;

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserEntity>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(IUserRepository userRepository, ILogger<AddUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<UserEntity> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling AddUserCommand for User: {@User}", request.User);

            var result = await _userRepository.AddUser(request.User, cancellationToken);

            _logger.LogInformation("User successfully added with Id: {UserId}", result.Id);

            return result;
        }
    }
}
