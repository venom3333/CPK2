using System;
using System.Collections.Generic;

namespace CPK.BasketModule.Entities
{
    public class Basket
    {
        private BasketState _state;

        public Basket(BasketId id, BasketState state)
        {
            if (id.Equals(default))
                throw new ArgumentNullException(nameof(id));
            Id = id;
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public BasketId Id { get; }

        public IReadOnlyCollection<BasketLine> Lines => _state.Lines;

        public void Add(BasketProduct basketProduct) => _state = _state.Add(basketProduct);

        public void Remove(Guid productId) => _state = _state.Remove(productId);

        public void Clear() => _state = _state.Clear();
    }
}
