using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Declare Repositories
        IVariantRepository Variant { get; }
        ISubVariantRepository SubVariant { get; }
        ICategoryRepository Category { get; }
        IProductItemRepository ProductItem { get; }
        IProductSizeRepository ProductSize { get; }

        // SaveChanges Method
        Task<int> CommitAsync(CancellationToken token);
    }
}
