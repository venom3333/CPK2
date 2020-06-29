using System;
using System.Collections.Generic;
using System.Linq;
using CPK.OrdersModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class OrderDto
    {
        public Guid Id { get; set; }
        public List<OrderLineDto> Lines { get; set; } = new List<OrderLineDto>();
        public string BuyerId { get; set; }
        public OrderStatus Status { get; set; }
        public string Address { get; set; }

        public OrderDto()
        {
            //For framework
        }

        public OrderDto(Order order)
        {
            Id = order.Id.Value;
            Lines = order.Lines.Select(l => new OrderLineDto(l, new ProductDto(l.Product, null), this)).ToList();
            BuyerId = order.Buyer.Id;
        }

        public Order ToOrder()
        {
            var state = (OrderState)(Status switch
            {
                OrderStatus.Created => new OrderCreated(),
                OrderStatus.Delivered => new OrderDelivered(),
                _ => throw new ArgumentOutOfRangeException(nameof(Status))
            });

            return new Order(
                new OrderId(Id),
                Lines.Select(l => new OrderLine(l.Product.ToOrderProduct().Entity, (uint)l.Quantity)),
                new Client(BuyerId),
                state,
                new Address(Address)
                );
        }
    }
}
