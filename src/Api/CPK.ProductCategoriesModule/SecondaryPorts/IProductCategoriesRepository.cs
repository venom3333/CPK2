using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.ProductCategoriesModule.Dto;
using CPK.ProductCategoriesModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.ProductCategoriesModule.SecondaryPorts
{
    public interface IProductCategoriesRepository
    {
        Task<List<ConcurrencyToken<ProductCategory>>> Get(ProductCategoriesFilter productCategoriesFilter);
        void Add(ProductCategory productCategory);
        Task Update(ConcurrencyToken<ProductCategory> productCategory);
        Task Remove(ConcurrencyToken<Id> id);
        Task<ConcurrencyToken<ProductCategory>> Get(Id id);
        Task<int> Count(ProductCategoriesFilter productCategoriesFilter);
    }
}