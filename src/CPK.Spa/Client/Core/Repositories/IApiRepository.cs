using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using MatBlazor;

namespace CPK.Spa.Client.Core.Repositories
{
    public interface IApiRepository
    {
        Task<(PageResultModel<ProductModel>, string)> GetFilteredProducts(ProductsFilterModel model);
        Task<(int, string)> AddToBasket(ProductModel product);
        Task<(BasketModel, string)> GetBasket();
        Task<(int, string)> Remove(Guid id);
        Task<(int, string)> ClearBasket();
        Task<(Guid, string)> CreateOrder(IEnumerable<LineModel> lines, string address);
        Task<(List<OrderModel>, string)> GetOrders();
        string GetFullUrl(string path);


        // Categories
        Task<(PageResultModel<ProductCategoryModel>, string)> GetFilteredProductCategories(ProductCategoriesFilterModel model);
        Task<(Guid, string)> CreateCategory(ProductCategoryModel model);
        Task<(int, string)> UpdateCategory(ProductCategoryModel model);
        Task<(int, string)> RemoveCategory(Guid id, string version);

        // Files
        Task<(Guid, string)> UploadFile(FileModel model);
        Task<(FileModel, string)> GetFile(Guid id);
    }
}