using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;


namespace MyServer.Infrastructure.Repositories
{
    public class UserRepository(ApplicationContextDB dbContext) : IUserRepository
    {   
       
        public async Task<UserEntity> GetUserById(Guid id)
        {
            try { 
                var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }

                return user;
            } catch { 
                throw new Exception($"An error occurred while retrieving the user with ID {id}.");
            }   
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsers()
        {
            try { 

                var users = await dbContext.Users.ToListAsync();
                if(users == null || !users.Any())
                {
                    throw new KeyNotFoundException("No users found.");
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving all users: {ex.Message}", ex);
            }
        }

        public async Task<UserEntity> AddUser(UserEntity user, CancellationToken cancellationToken)
        {

            try {
                user.Id = Guid.NewGuid();
                dbContext.Users.Add(user);

                await dbContext.SaveChangesAsync(cancellationToken);
                return user;
            
            } catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the user: {ex.Message}", ex);
            }
        }

        public async Task<UserEntity?> UpdateUser(Guid id, UserEntity updatedUser)
        {

            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user is null)
                {
                    return null; 
                }

                user.Username = updatedUser.Username;
                user.Email = updatedUser.Email;
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.Role = updatedUser.Role;
                user.IsActive = updatedUser.IsActive;
                user.EmailConfirmed = updatedUser.EmailConfirmed;
                user.LastLogin = updatedUser.LastLogin;

                await dbContext.SaveChangesAsync();
                return user;

            } catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user: {ex.Message}", ex);
            }
        }

        public async Task<UserEntity> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            try { 
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user is null)
                {
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync(cancellationToken);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the user: {ex.Message}", ex);
            }
        }










    }
}
