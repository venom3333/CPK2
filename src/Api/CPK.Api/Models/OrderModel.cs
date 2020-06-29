using System;
using System.Collections.Generic;
using System.Linq;
using CPK.OrdersModule.Entities;

namespace CPK.Api.Models
{
    public sealed class OrderModel
    {
        public Guid Id { get; }
        public List<LineModel> Lines { get; set; } = new List<LineModel>();
        public string Buyer { get; set; }
        public OrderStatus Status { get; set; }
        public string Address { get; set; }

        public OrderModel()
        {

        }

        public OrderModel(Order order)
        {
            Id = order.Id.Value;
            Lines = order.Lines.Select(l => new LineModel(l)).ToList();
            Buyer = order.Buyer.Id;
            Status = order.State;
            Address = order.Address.Value;
        }
    }
}
