using MyServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<CategoryEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoryEntity> AddAsync(CategoryEntity category, CancellationToken cancellationToken = default);
        Task<CategoryEntity?> UpdateAsync(Guid id, CategoryEntity updatedCategory, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
