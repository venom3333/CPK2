using System;

namespace CPK.BasketModule.Entities
{
    public readonly struct BasketProduct
    {
        public Guid Id { get; }
        public string Title { get; }
        public decimal Price { get; }

        public BasketProduct(Guid id, string title, decimal price)
        {
            Id = id;
            Title = title;
            Price = price;
        }
    }
}
