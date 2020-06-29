using System.Threading.Tasks;
using CPK.BasketModule.Entities;

namespace CPK.BasketModule.SecondaryPorts
{
    public interface IBasketRepository
    {
        Task<Basket> Get(BasketId id);
        Task Save(Basket basket);
    }
}
