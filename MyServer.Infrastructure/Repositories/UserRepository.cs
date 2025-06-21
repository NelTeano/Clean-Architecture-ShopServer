using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;


namespace MyServer.Infrastructure.Repositories
{
    internal class UserRepository(ApplicationContextDB dbContext) : IUserRepository
    {   
       
        public async Task<UserEntity> GetUserById(Guid id)
        {
            var user = await dbContext.User.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return user;
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsers()
        {
            return await dbContext.User.ToListAsync();
        }

        public async Task<UserEntity> AddUser(UserEntity user)
        {
            user.Id = Guid.NewGuid();
            dbContext.User.Add(user);

            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity> UpdateUser(Guid id, UserEntity user)
        {
            var User = await dbContext.User.FirstOrDefaultAsync(u => u.Id == id);

            if(User is not null)
            {
                User.Username = user.Username;
                User.Email = user.Email;

                await dbContext.SaveChangesAsync();
                return User;
            }

            return user;
        }

        public async Task<UserEntity> DeleteUser(Guid id)
        {
            try { 
                var user = await dbContext.User.FirstOrDefaultAsync(x => x.Id == id);
                if (user is null)
                {
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }
                dbContext.User.Remove(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the user: {ex.Message}", ex);
            }
        }










    }
}
