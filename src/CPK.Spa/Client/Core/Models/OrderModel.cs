using System;
using System.Collections.Generic;

namespace CPK.Spa.Client.Core.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string Buyer { get; set; }
        public OrderStatus Status { get; set; }
        public List<LineModel> Lines { get; set; } = new List<LineModel>();
        public string Address { get; set; }
    }
}