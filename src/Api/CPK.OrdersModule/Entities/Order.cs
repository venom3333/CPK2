using System;
using System.Collections.Generic;
using System.Linq;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.OrdersModule.Entities
{
    public class Order
    {
        private readonly List<OrderLine> _lines;
        private OrderState _state;

        public Order(IEnumerable<OrderLine> lines, Client buyer, Address address) : this(new OrderId(Guid.NewGuid()), lines, buyer, new OrderCreated(), address) { }

        public Order(OrderId id, IEnumerable<OrderLine> lines, Client buyer, OrderState state, Address address)
        {
            Id = id;
            Buyer = buyer;
            Address = address;
            _state = state;
            _lines = lines?.ToList();
            Validate();
        }

        private void Validate()
        {
            Validator
                .Begin(Id, nameof(Id))
                .NotDefault()
                .Map(Buyer, nameof(Buyer))
                .NotDefault()
                .Map(_state, nameof(_state))
                .NotNull()
                .Map(_lines, nameof(_lines))
                .NotNull()
                .NotEmpty()
                .Map(Address, nameof(Address))
                .NotNull()
                .ThrowApiException(nameof(Order), nameof(Validate));
        }

        public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

        public OrderId Id { get; }
        public Client Buyer { get; }
        public Address Address { get; }

        public OrderStatus State => _state.Status;

        public void Delivered() => _state = _state.Delivered();
    }
}
