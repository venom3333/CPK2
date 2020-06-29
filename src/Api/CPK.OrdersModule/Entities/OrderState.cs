namespace CPK.OrdersModule.Entities
{
    public abstract class OrderState
    {
        public OrderStatus Status { get; }

        protected OrderState(OrderStatus status)
        {
            Status = status;
        }

        public abstract OrderState Delivered();
    }
}
