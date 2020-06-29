using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.OrdersModule.Entities;

namespace CPK.OrdersModule.SecondaryPorts
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Task<Order> Get(OrderId id);
        Task<List<Order>> Get(Client buyer);
        void Update(Order order);
    }
}
