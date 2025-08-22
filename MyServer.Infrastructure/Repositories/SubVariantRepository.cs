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
    public class SubVariantRepository(ApplicationContextDB _context) : ISubVariantRepository
    {
        public async Task<SubVariantEntity> GetById(int Id, CancellationToken token)
        {
            try
            {
                var subVariant = await _context.SubVariants.FirstOrDefaultAsync(s => s.Id == Id);

                if(subVariant == null)
                {
                    throw new KeyNotFoundException($"sub varaint not found matches id: {Id}");
                }

                return subVariant;
            }
            catch(Exception ex)
            {
                throw new Exception("Error get id sub variants", ex);
            }
        }

        public async Task<IEnumerable<SubVariantEntity>> GetAll(CancellationToken token){
            
            try
            {
                var subVariant = await _context.SubVariants.ToListAsync(token); 

                if (subVariant == null)
                {
                    return Enumerable.Empty<SubVariantEntity>();
                }

                return subVariant;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all sub variants", ex);
            }
        }
        public async Task<SubVariantEntity> Add(SubVariantEntity subVariant, CancellationToken token){

            try
            {
                _context.SubVariants.Add(subVariant);
                await _context.SaveChangesAsync(token);

                return subVariant;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding sub variants", ex);
            }
        }
        public async Task<SubVariantEntity> Remove(int Id, CancellationToken token){
            try
            {
                var subVariant = await _context.SubVariants.FirstOrDefaultAsync(s => s.Id == Id);

                if (subVariant == null)
                {
                    throw new KeyNotFoundException($"sub varaint not found matches id: {Id}");
                }

                _context.Remove(subVariant);
                await _context.SaveChangesAsync(token);

                return subVariant;

            }
            catch (Exception ex)
            {
                throw new Exception("Error removing sub variants", ex);
            }
        }
        public async Task<SubVariantEntity> Update(int Id, SubVariantEntity updatedSubVariant, CancellationToken token){
            try
            {
                var subVariant = await _context.SubVariants.FirstOrDefaultAsync(s => s.Id == Id);
                
                if (subVariant == null)
                {
                    throw new KeyNotFoundException($"sub varaint not found matches id: {Id}");
                }

                subVariant.Name = updatedSubVariant.Name;
                subVariant.VariantId = updatedSubVariant.VariantId;

                await _context.SaveChangesAsync(token);

                return subVariant;

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating sub variants", ex);
            }
        }

    }
}
