using CPK.BasketModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal sealed class BasketUow : UnitOfWorkBase, IBasketUow
    {
        public BasketUow(CpkContext context) : base(context)
        {
        }
    }
}
