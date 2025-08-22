using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;

namespace MyServer.Infrastructure.Repositories
{
    public class ProductSizeRepository(ApplicationContextDB _context) : IProductSizeRepository
    {
        public async Task<ProductSizeEntity> GetById(int Id, CancellationToken token)
        {
            try
            {
                var productSize = await _context.ProductSizes.FirstOrDefaultAsync(p => p.Id == Id);

                if (productSize == null)
                {
                    throw new KeyNotFoundException($"product size not found matches id: {Id}");
                }

                return productSize;
            }
            catch (Exception ex)
            {
                throw new Exception("Error get product size", ex);
            }
        }

        public async Task<IEnumerable<ProductSizeEntity>> GetAll(CancellationToken token)
        {

            try
            {
                var productSizes = await _context.ProductSizes.ToListAsync(token);

                if (productSizes == null)
                {
                    return Enumerable.Empty<ProductSizeEntity>();
                }

                return productSizes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting product sizes", ex);
            }
        }
        public async Task<ProductSizeEntity> Add(ProductSizeEntity productSize, CancellationToken token)
        {

            try
            {
                _context.ProductSizes.Add(productSize);
                await _context.SaveChangesAsync(token);

                return productSize;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product size", ex);
            }
        }
        public async Task<ProductSizeEntity> Remove(int Id, CancellationToken token)
        {
            try
            {
                var removeProductsize = await _context.ProductSizes.FirstOrDefaultAsync(c => c.Id == Id);

                if (removeProductsize == null)
                {
                    throw new KeyNotFoundException($"product size not found matches id: {Id}");
                }

                _context.Remove(removeProductsize);
                await _context.SaveChangesAsync(token);

                return removeProductsize;

            }
            catch (Exception ex)
            {
                throw new Exception("Error removing product size", ex);
            }
        }
        public async Task<ProductSizeEntity> Update(int Id, ProductSizeEntity updatedProductsize, CancellationToken token)
        {
            try
            {
                var productsize = await _context.ProductSizes.FirstOrDefaultAsync(s => s.Id == Id);

                if (productsize == null)
                {
                    throw new KeyNotFoundException($"product size not found matches id: {Id}");
                }

                productsize.Size = updatedProductsize.Size;
                productsize.Price = updatedProductsize.Price;
                productsize.ProductItemId = updatedProductsize.ProductItemId;


                await _context.SaveChangesAsync(token);

                return productsize;

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product size", ex);
            }
        }
    }
}
