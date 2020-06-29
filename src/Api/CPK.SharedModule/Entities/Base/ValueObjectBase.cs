using System;
using System.Collections.Generic;

namespace CPK.SharedModule.Entities.Base
{
    public abstract class ValueObjectBase
    {

        public static bool operator ==(ValueObjectBase left, ValueObjectBase right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        public static bool operator !=(ValueObjectBase left, ValueObjectBase right)
        {
            return !(left == right);
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            ValueObjectBase other = (ValueObjectBase)obj;
            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            var code = new HashCode();
            foreach (var value in GetAtomicValues())
            {
                code.Add(value);
            }
            return code.ToHashCode();
        }
    }
}
