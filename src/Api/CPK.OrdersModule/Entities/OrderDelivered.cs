using CPK.SharedModule.Entities;

namespace CPK.OrdersModule.Entities
{
    public sealed class OrderDelivered : OrderState
    {
        public OrderDelivered() : base(OrderStatus.Delivered)
        {

        }

        public override OrderState Delivered()
        {
            throw new ApiException(ApiExceptionCode.OrderAlreadyDelivered);
        }
    }
}
