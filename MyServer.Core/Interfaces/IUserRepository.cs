using MyServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserById(Guid id);
        Task<IEnumerable<UserEntity>> GetAllUsers();
        Task<UserEntity> AddUser(UserEntity user);
        Task<UserEntity> UpdateUser(Guid id, UserEntity user);
        Task<UserEntity> DeleteUser(Guid id);
    }
}
