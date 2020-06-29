using System.Collections.Generic;
using System.Linq;
using CPK.BasketModule.Entities;

namespace CPK.Api.Models
{
    public sealed class BasketModel
    {
        public List<LineModel> Lines { get; set; } = new List<LineModel>();

        public BasketModel(Basket basket)
        {
            Lines = basket.Lines.Select(l => new LineModel(l)).ToList();
        }

        public BasketModel()
        {

        }

        public Basket ToBasket(BasketId id, int maxSize) => new Basket(id, BasketState.Create(Lines.Select(l => l.ToBasketLine()), maxSize));
    }
}
