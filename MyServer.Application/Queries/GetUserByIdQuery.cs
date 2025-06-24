using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Queries
{

    public record GetUserByIdQuery(UserEntity User) : IRequest<UserEntity>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserEntity>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEntity> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.User.Id);
            return user ?? throw new KeyNotFoundException($"User with ID {request.User.Id} not found.");
        }
    }
}
