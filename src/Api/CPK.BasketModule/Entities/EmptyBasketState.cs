using System;
using System.Collections.Generic;
using CPK.SharedModule.Entities;

namespace CPK.BasketModule.Entities
{
    public class EmptyBasketState : BasketState
    {

        public override BasketState Add(BasketProduct basketProduct)
        {
            _lines.Add(new BasketLine(basketProduct, 1));
            return new NormalBasketState(_lines, _maxSize);
        }

        public override BasketState Remove(Guid productId)
        {
            throw new ApiException(ApiExceptionCode.BasketIsEmpty);
        }

        public override BasketState Clear()
        {
            throw new ApiException(ApiExceptionCode.BasketIsEmpty);
        }

        public EmptyBasketState(int maxSize) : base(new List<BasketLine>(), maxSize)
        {
        }
    }
}
