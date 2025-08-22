using MyServer.Core.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
    public interface ISubVariantRepository
    {
        Task<SubVariantEntity> GetById(int Id, CancellationToken token);
        Task<IEnumerable<SubVariantEntity>> GetAll(CancellationToken token);
        Task<SubVariantEntity> Add(SubVariantEntity subVariant, CancellationToken token);
        Task<SubVariantEntity> Remove(int Id, CancellationToken token);
        Task<SubVariantEntity> Update(int Id, SubVariantEntity subVariant, CancellationToken token);
    }
}
