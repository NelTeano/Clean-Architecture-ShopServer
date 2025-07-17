using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Commands.User
{
    public record UpdateUserCommand(
        Guid Id,
        string Username,
        string Email,
        string? FirstName,
        string? LastName,
        string? PhoneNumber,
        bool EmailConfirmed,
        bool IsActive,
        DateTime CreatedOn,
        DateTime? LastLogin,
        string? Role
    ) : IRequest<UserEntity>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserEntity>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEntity> Handle(UpdateUserCommand user, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserById(user.Id);

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.EmailConfirmed = user.EmailConfirmed;
            existingUser.IsActive = user.IsActive;
            existingUser.CreatedOn = user.CreatedOn;
            existingUser.LastLogin = user.LastLogin;
            existingUser.Role = user.Role;

            var updatedUser = await _userRepository.UpdateUser(user.Id, existingUser);
            if (updatedUser is null)
                throw new InvalidOperationException("User not found or update failed.");

            return updatedUser;
        }
    }
}
