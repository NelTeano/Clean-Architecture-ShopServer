using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;
using MyServer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyServer.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationContextDB _dbContext;

        public CategoryRepository(ApplicationContextDB dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<CategoryEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var category = await _dbContext.Category.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found.");
                }
                return category;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while retrieving the category.", ex);


                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Category.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while retrieving all categories.", ex);
            }
        }

        public async Task<CategoryEntity> AddAsync(CategoryEntity category, CancellationToken cancellationToken = default)
        {
            try
            {
                var id = Guid.NewGuid();
                category.Id = id; // Ensure the category has a new ID
                var savedCategory = await _dbContext.Category.AddAsync(category, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category.", ex);
            }
        }


        public async Task<CategoryEntity?> UpdateAsync(Guid id, CategoryEntity updatedCategory, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingCategory = await _dbContext.Category.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
                if (existingCategory == null)
                {
                    return null; // Category not found
                }

                if (updatedCategory.Name != null)
                    existingCategory.Name = updatedCategory.Name;

                if (updatedCategory.Description != null)
                    existingCategory.Description = updatedCategory.Description;

                existingCategory.IsActive = updatedCategory.IsActive;

                await _dbContext.SaveChangesAsync(cancellationToken);
                return existingCategory;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }


        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var category = await _dbContext.Category.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

                if (category == null)
                {
                    return false; // Category not found
                }

                _dbContext.Category.Remove(category);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }
    }
}
