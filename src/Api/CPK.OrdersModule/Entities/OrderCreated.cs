namespace CPK.OrdersModule.Entities
{
    public sealed class OrderCreated : OrderState
    {
        public OrderCreated() : base(OrderStatus.Created)
        {

        }

        public override OrderState Delivered()
        {
            return new OrderDelivered();
        }
    }
}
