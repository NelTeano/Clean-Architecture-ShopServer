using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Infrastructure.Repositories
{
    public class CategoryRepository(ApplicationContextDB _context) : ICategoryRepository
    {
        public async Task<CategoryEntity> GetById(int Id, CancellationToken token)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);

                if (category == null)
                {
                    throw new KeyNotFoundException($"category not found matches id: {Id}");
                }

                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error get id category", ex);
            }
        }

        public async Task<IEnumerable<CategoryEntity>> GetAll(CancellationToken token)
        {

            try
            {
                var categories = await _context.Categories.ToListAsync(token);

                if (categories == null)
                {
                    return Enumerable.Empty<CategoryEntity>();
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all categories", ex);
            }
        }
        public async Task<CategoryEntity> Add(CategoryEntity category, CancellationToken token)
        {

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync(token);

                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category", ex);
            }
        }
        public async Task<CategoryEntity> Remove(int Id, CancellationToken token)
        {
            try
            {
                var removeCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);

                if (removeCategory == null)
                {
                    throw new KeyNotFoundException($"category not found matches id: {Id}");
                }

                _context.Remove(removeCategory);
                await _context.SaveChangesAsync(token);

                return removeCategory;

            }
            catch (Exception ex)
            {
                throw new Exception("Error removing sub variants", ex);
            }
        }
        public async Task<CategoryEntity> Update(int Id, CategoryEntity updatedCategory, CancellationToken token)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == Id);

                if (category == null)
                {
                    throw new KeyNotFoundException($"category not found matches id: {Id}");
                }

                category.Name = updatedCategory.Name;
                category.SubVariantId = updatedCategory.SubVariantId;

                await _context.SaveChangesAsync(token);

                return category;

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category", ex);
            }
        }
    }
}
