using System.Collections.Generic;
using CPK.SharedModule.Entities;

namespace CPK.BasketModule.Entities
{
    public class FullBasketState : NormalBasketState
    {
        public FullBasketState(IEnumerable<BasketLine> lines, int maxSize) : base(lines, maxSize)
        {
        }

        public override BasketState Add(BasketProduct basketProduct)
        {
            throw new ApiException(ApiExceptionCode.BasketIsFull);
        }
    }
}
