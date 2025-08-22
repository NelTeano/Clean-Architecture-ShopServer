using MyServer.Core.Entities.ProductEntities;


namespace MyServer.Core.Interfaces
{
    public interface IVariantRepository
    {

        Task<VariantEntity> GetById(int Id, CancellationToken token);
        Task<IEnumerable<VariantEntity>> GetAll(CancellationToken token);
        Task<VariantEntity> Add(VariantEntity variant, CancellationToken token);
        Task<VariantEntity> Remove(int Id, CancellationToken token);
        Task<VariantEntity> Update(int Id, VariantEntity variant, CancellationToken token);

    }
}
