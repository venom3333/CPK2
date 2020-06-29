using CPK.SharedModule.Entities;

namespace CPK.OrdersModule.Entities
{
    public class OrderLine : Line<OrderProduct>
    {
        public Order Order { get; }

        public OrderLine(OrderProduct orderProduct, uint quantity) : base(orderProduct, quantity)
        {
        }
    }
}
