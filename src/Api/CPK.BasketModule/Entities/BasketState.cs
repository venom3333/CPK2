using System;
using System.Collections.Generic;
using System.Linq;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.BasketModule.Entities
{
    public abstract class BasketState
    {
        protected readonly int _maxSize;
        protected readonly List<BasketLine> _lines;

        protected BasketState(IEnumerable<BasketLine> lines, int maxSize)
        {
            _maxSize = maxSize;
            _lines = lines?.ToList();
            Validate();
        }

        private void Validate()
        {
            Validator.Begin(_maxSize, nameof(_maxSize))
                .IsGreater(0)
                .Map(_lines, nameof(_lines))
                .NotNull()
                .LengthIsLess(_maxSize)
                .ThrowApiException(nameof(BasketState), nameof(BasketState));
        }

        public abstract BasketState Add(BasketProduct basketProduct);

        public abstract BasketState Remove(Guid productId);

        public abstract BasketState Clear();

        public IReadOnlyCollection<BasketLine> Lines => _lines.AsReadOnly();

        public static BasketState Create(IEnumerable<BasketLine> lines, int maxSize)
        {
            var list = lines.ToList();
            if (list.Count == 0)
                return new EmptyBasketState(maxSize);
            if (list.Count >= maxSize)
                return new FullBasketState(list, maxSize);
            return new NormalBasketState(list, maxSize);
        }
    }
}
