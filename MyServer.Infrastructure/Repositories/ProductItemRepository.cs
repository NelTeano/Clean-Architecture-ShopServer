using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;

namespace MyServer.Infrastructure.Repositories
{
    public class ProductItemRepository(ApplicationContextDB _context) : IProductItemRepository
    {
        public async Task<ProductItemEntity> GetById(int Id, CancellationToken token)
        {
            try
            {
                var productItem = await _context.ProductItems.FirstOrDefaultAsync(i => i.Id == Id);

                if (productItem == null)
                {
                    throw new KeyNotFoundException($"product item not found matches id: {Id}");
                }

                return productItem;
            }
            catch (Exception ex)
            {
                throw new Exception("Error get product item", ex);
            }
        }

        public async Task<IEnumerable<ProductItemEntity>> GetAll(CancellationToken token)
        {

            try
            {
                var productItems = await _context.ProductItems.ToListAsync(token);

                if (productItems == null)
                {
                    return Enumerable.Empty<ProductItemEntity>();
                }

                return productItems;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting product items", ex);
            }
        }
        public async Task<ProductItemEntity> Add(ProductItemEntity productItem, CancellationToken token)
        {

            try
            {
                _context.ProductItems.Add(productItem);
                await _context.SaveChangesAsync(token);

                return productItem;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product items", ex);
            }
        }
        public async Task<ProductItemEntity> Remove(int Id, CancellationToken token)
        {
            try
            {
                var removeProductItem = await _context.ProductItems.FirstOrDefaultAsync(c => c.Id == Id);

                if (removeProductItem == null)
                {
                    throw new KeyNotFoundException($"product item not found matches id: {Id}");
                }

                _context.Remove(removeProductItem);
                await _context.SaveChangesAsync(token);

                return removeProductItem;

            }
            catch (Exception ex)
            {
                throw new Exception("Error removing product item", ex);
            }
        }
        public async Task<ProductItemEntity> Update(int Id, ProductItemEntity updatedProductItem, CancellationToken token)
        {
            try
            {
                var productItem = await _context.ProductItems.FirstOrDefaultAsync(s => s.Id == Id);

                if (productItem == null)
                {
                    throw new KeyNotFoundException($"product item not found matches id: {Id}");
                }

                productItem.Name = updatedProductItem.Name;
                productItem.Image = updatedProductItem.Image;
                productItem.Description = updatedProductItem.Description;
                productItem.Quantity = updatedProductItem.Quantity;
                productItem.CategoryId = updatedProductItem.CategoryId;

                await _context.SaveChangesAsync(token);

                return productItem;

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product item", ex);
            }
        }
    }
}
