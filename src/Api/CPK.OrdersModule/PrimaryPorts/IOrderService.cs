using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.OrdersModule.Entities;

namespace CPK.OrdersModule.PrimaryPorts
{
    public interface IOrderService
    {
        Task<OrderId> Create(Order request);
        Task<Order> Get(OrderId request);
        Task<List<Order>> Get(Client request);
        Task<int> Delivered(OrderId id);
    }
}
