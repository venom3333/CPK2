using System;

namespace CPK.OrdersModule.Entities
{
    public readonly struct OrderId : IEquatable<OrderId>
    {
        public bool Equals(OrderId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is OrderId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public Guid Value { get; }

        public OrderId(Guid value)
        {
            if (value == default)
                throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }

        public static bool operator ==(OrderId lhs, OrderId rhs) => lhs.Value == rhs.Value;
        public static bool operator !=(OrderId lhs, OrderId rhs) => !(lhs == rhs);
    }
}
