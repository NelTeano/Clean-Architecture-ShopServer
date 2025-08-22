using MyServer.Core.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<CategoryEntity> GetById(int Id, CancellationToken token);
        Task<IEnumerable<CategoryEntity>> GetAll(CancellationToken token);
        Task<CategoryEntity> Add(CategoryEntity category, CancellationToken token);
        Task<CategoryEntity> Remove(int Id, CancellationToken token);
        Task<CategoryEntity> Update(int Id, CategoryEntity category, CancellationToken token);
    }
}
