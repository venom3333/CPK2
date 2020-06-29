using CPK.SharedModule.Entities;

namespace CPK.BasketModule.Entities
{
    public class BasketLine : Line<BasketProduct>
    {

        public BasketLine(BasketProduct basketProduct, uint quantity) : base(basketProduct, quantity)
        {
        }
    }
}
