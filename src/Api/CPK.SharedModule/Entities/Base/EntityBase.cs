using System;
using System.Collections.Generic;

namespace CPK.SharedModule.Entities.Base
{
    public abstract class EntityBase<T> where T : IEquatable<T>
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public abstract T Id { get; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => this._domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (GetType() != obj.GetType())
                return false;
            EntityBase<T> item = (EntityBase<T>)obj;
            if (ReferenceEquals(item.Id, null))
                return false;
            return item.Id.Equals(Id);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(EntityBase<T> left, EntityBase<T> right)
        {
            if (Object.Equals(left, null))
                return Object.Equals(right, null);
            else
                return left.Equals(right);
        }

        public static bool operator !=(EntityBase<T> left, EntityBase<T> right)
        {
            return !(left == right);
        }
    }
}
