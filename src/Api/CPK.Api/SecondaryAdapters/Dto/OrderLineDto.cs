using System;
using CPK.OrdersModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class OrderLineDto
    {
        public OrderDto Order { get; set; }
        public Guid OrderId { get; set; }
        public ProductDto Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public OrderLineDto()
        {

        }

        public OrderLineDto(OrderLine line, ProductDto product, OrderDto order)
        {
            Order = order;
            Product = product;
            OrderId = order.Id;
            ProductId = product.Id;
            Quantity = (int)line.Quantity;

        }
    }
}
