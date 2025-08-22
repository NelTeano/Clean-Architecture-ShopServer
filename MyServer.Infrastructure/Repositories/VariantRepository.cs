using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;

namespace MyServer.Infrastructure.Repositories
{
    public class VariantRepository(ApplicationContextDB context) : IVariantRepository
    {

        public async Task<VariantEntity> GetById(int Id, CancellationToken token)
        {
            try
            {
                var variant = await context.Variants.FirstOrDefaultAsync(v => v.Id == Id, token);

                if (variant == null)
                {
                    throw new KeyNotFoundException($"No Variant matches the id: {Id} that you provided");
                }

                return variant;
            }
            catch (Exception ex)
            {
                throw new Exception($"No Variant matches the id: {Id} that you provided", ex);
            }
        }

        public async Task<IEnumerable<VariantEntity>> GetAll(CancellationToken token)
        {
            try
            {
                var Variant = await context.Variants.ToListAsync(token);

                if(Variant == null)
                {
                    return Enumerable.Empty<VariantEntity>();
                }

                return Variant;
            }
            catch (Exception ex)
            {
                throw new Exception($"No Variants Found", ex);
            }
        }

        public async Task<VariantEntity> Add(VariantEntity variant, CancellationToken token)
        {
            try
            {
                context.Variants.Add(variant);
                await context.SaveChangesAsync(token);
                return variant;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Adding Variant", ex);
            }
        }

        public async Task<VariantEntity> Remove(int Id, CancellationToken token)
        {
            try
            {
                var variant = await context.Variants.FirstOrDefaultAsync(v => v.Id == Id, token);
                if (variant == null)
                {
                    throw new KeyNotFoundException($"No Variant matches the id: {Id} that you provided");
                }

                context.Variants.Remove(variant);
                await context.SaveChangesAsync(token);
                return variant;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Removing Variant", ex);
            }
        }

        public async Task<VariantEntity> Update(int Id, VariantEntity updatedVariant, CancellationToken token)
        {
            try
            {
                var variant = await context.Variants.FirstOrDefaultAsync(v => v.Id == Id);
                if (variant == null)
                {
                    throw new KeyNotFoundException($"No Variant matches the id: {Id} that you provided");
                }

                variant.VariantName = updatedVariant.VariantName;
                await context.SaveChangesAsync(token);

                return variant;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error Updating Variant", ex);
            }
        }
    }
}
