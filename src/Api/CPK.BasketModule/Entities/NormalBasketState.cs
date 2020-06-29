using System;
using System.Collections.Generic;
using System.Linq;
using CPK.SharedModule.Entities;

namespace CPK.BasketModule.Entities
{
    public class NormalBasketState : BasketState
    {

        public override BasketState Add(BasketProduct basketProduct)
        {
            var line = _lines.FirstOrDefault(p => p.Product.Id == basketProduct.Id);
            if (line == null)
            {
                _lines.Add(new BasketLine(basketProduct, 1));
            }
            else
            {
                line.Add();
            }

            if (_lines.Count == _maxSize)
                return new FullBasketState(_lines, _maxSize);
            return new NormalBasketState(_lines, _maxSize);
        }

        public override BasketState Remove(Guid productId)
        {
            var line = _lines.FirstOrDefault(p => p.Product.Id == productId);
            if (line == null)
            {
                var exception = new ApiException(ApiExceptionCode.ProductNotExist);
                exception.Data["id"] = productId;
                throw exception;
            }

            if (line.Quantity == 1)
            {
                _lines.Remove(line);
            }
            else
            {
                line.Remove();
            }


            if (_lines.Count == 0)
                return new EmptyBasketState(_maxSize);
            return new NormalBasketState(_lines, _maxSize);
        }

        public override BasketState Clear() => new EmptyBasketState(_maxSize);

        public NormalBasketState(IEnumerable<BasketLine> lines, int maxSize) : base(lines, maxSize)
        {
        }
    }
}
