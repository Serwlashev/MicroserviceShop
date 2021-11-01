using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain.Common
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if(left is null ^ right is null)
            {
                return false;
            }

            return left?.Equals(right) != false;
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqalityComponents();

        public override bool Equals(object obj)
        {
            if(obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            return GetEqalityComponents().SequenceEqual(other.GetEqalityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqalityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
