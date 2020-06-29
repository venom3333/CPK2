using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.OrdersModule.Entities
{
    public readonly struct Client : IEquatable<Client>
    {
        public string Id { get; }

        public Client(string id)
        {
            Validator.Begin(id, nameof(id))
                .NotNull()
                .NotWhiteSpace()
                .ThrowApiException(nameof(Client), nameof(Client));
            Id = id;
        }

        public bool Equals(Client other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            return obj is Client other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
