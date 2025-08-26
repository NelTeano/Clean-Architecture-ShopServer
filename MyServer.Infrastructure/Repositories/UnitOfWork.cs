using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;
using MyServer.Core.Interfaces;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContextDB _context;

    public IVariantRepository Variant { get; }
    public ISubVariantRepository SubVariant { get; }
    public ICategoryRepository Category { get; }
    public IProductItemRepository ProductItem { get; }
    public IProductSizeRepository ProductSize { get; }

    public UnitOfWork(
        ApplicationContextDB context,
        IVariantRepository variantRepository,
        ISubVariantRepository subVariantRepository,
        ICategoryRepository categoryRepository,
        IProductItemRepository productItemRepository,
        IProductSizeRepository productSizeRepository
    )
    {
        _context = context;
        Variant = variantRepository;
        SubVariant = subVariantRepository;
        Category = categoryRepository;
        ProductItem = productItemRepository;
        ProductSize = productSizeRepository;
    }

    public async Task<int> CommitAsync(CancellationToken token)
    {
        return await _context.SaveChangesAsync(token);
    }

    public void Dispose() => _context.Dispose();
}
