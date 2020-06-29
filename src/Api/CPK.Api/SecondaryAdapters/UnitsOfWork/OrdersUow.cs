using CPK.OrdersModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal sealed class OrdersUow : UnitOfWorkBase, IOrdersUow
    {
        public OrdersUow(CpkContext context) : base(context)
        {
        }
    }
}
