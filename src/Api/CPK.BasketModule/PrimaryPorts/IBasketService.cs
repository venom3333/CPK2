using System;
using System.Threading.Tasks;
using CPK.BasketModule.Entities;

namespace CPK.BasketModule.PrimaryPorts
{
    public interface IBasketService
    {
        Task<int> AddProduct(BasketId id, BasketProduct basketProduct);
        Task<int> Clear(BasketId id);
        Task<Basket> Get(BasketId id);
        Task<int> RemoveProduct(BasketId id, Guid productId);
    }
}
