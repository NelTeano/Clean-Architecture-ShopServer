using MyServer.Core.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
    public interface IProductSizeRepository
    {
        Task<ProductSizeEntity> GetById(int Id, CancellationToken token);
        Task<IEnumerable<ProductSizeEntity>> GetAll(CancellationToken token);
        Task<ProductSizeEntity> Add(ProductSizeEntity size, CancellationToken token);
        Task<ProductSizeEntity> Remove(int Id, CancellationToken token);
        Task<ProductSizeEntity> Update(int Id, ProductSizeEntity size, CancellationToken token);
    }
}
