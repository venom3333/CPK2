using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;

namespace CPK.Spa.Client.Core.Repositories
{
    public interface IApiRepository
    {
        Task<(PageResultModel<ProductModel>, string)> GetFiltered(ProductsFilterModel model);
        Task<(int, string)> AddToBasket(ProductModel product);
        Task<(BasketModel, string)> GetBasket();
        Task<(int, string)> Remove(Guid id);
        Task<(int, string)> ClearBasket();
        Task<(Guid, string)> CreateOrder(IEnumerable<LineModel> lines, string address);
        Task<(List<OrderModel>, string)> GetOrders();
        string GetFullUrl(string path);
    }
}