using MyServer.Core.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
    public interface IProductItemRepository
    {
        Task<ProductItemEntity> GetById(int Id, CancellationToken token);
        Task<IEnumerable<ProductItemEntity>> GetAll(CancellationToken token);
        Task<ProductItemEntity> Add(ProductItemEntity item, CancellationToken token);
        Task<ProductItemEntity> Remove(int Id, CancellationToken token);
        Task<ProductItemEntity> Update(int Id, ProductItemEntity item, CancellationToken token);
    }
}
