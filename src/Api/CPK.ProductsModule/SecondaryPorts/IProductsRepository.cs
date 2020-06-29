using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.SharedModule;

namespace CPK.ProductsModule.SecondaryPorts
{
    public interface IProductsRepository
    {
        Task<List<ConcurrencyToken<Product>>> Get(ProductsFilter productsFilter);
        void Add(Product product);
        Task Update(ConcurrencyToken<Product> product);
        Task Remove(ConcurrencyToken<ProductId> id);
        Task<ConcurrencyToken<Product>> Get(ProductId id);
        Task<int> Count(ProductsFilter productsFilter);
    }
}
