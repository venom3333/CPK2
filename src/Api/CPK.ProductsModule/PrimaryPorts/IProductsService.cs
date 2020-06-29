using System.Threading.Tasks;
using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.SharedModule;

namespace CPK.ProductsModule.PrimaryPorts
{
    public interface IProductsService
    {
        Task<int> Add(Product request);
        Task<PageResult<ConcurrencyToken<Product>>> Get(ProductsFilter request);
        Task<int> Remove(ConcurrencyToken<ProductId> request);
        Task<int> Update(ConcurrencyToken<Product> request);
    }
}
