using System;

namespace CPK.OrdersModule.Entities
{
    public readonly struct OrderProduct
    {
        public Guid Id { get; }
        public string Title { get; }
        public decimal Price { get; }
        public OrderProduct(Guid id, string title, decimal price)
        {
            Id = id;
            Title = title;
            Price = price;
        }
    }
}
