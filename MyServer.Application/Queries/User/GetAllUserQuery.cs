
using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;

namespace MyServer.Application.Queries.User
{
    public record GetAllUserQuery() : IRequest <IEnumerable<UserEntity>>;

    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserEntity>>
    {
        private readonly IUserRepository _userRepository;
        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserEntity>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllUsers();
        }
    }
}
