using System.Threading.Tasks;
using CPK.ProductCategoriesModule.Dto;
using CPK.ProductCategoriesModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.ProductCategoriesModule.PrimaryPorts
{
    public interface IProductCategoriesService
    {
        Task<int> Add(ProductCategory request);
        Task<PageResult<ConcurrencyToken<ProductCategory>>> Get(ProductCategoriesFilter request);
        Task<int> Remove(ConcurrencyToken<Id> request);
        Task<int> Update(ConcurrencyToken<ProductCategory> request);
    }
}