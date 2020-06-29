using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.SharedModule
{
    public readonly struct PageFilter
    {
        public uint Skip { get; }
        public uint Take { get; }
        public uint MaxTake { get; }

        public PageFilter(uint skip, uint take, uint maxTake)
        {
            Validator.Begin(take, nameof(take))
                .IsGreater<uint>(0)
                .Map(maxTake, nameof(maxTake))
                .IsGreater<uint>(0)
                .ThrowApiException(nameof(PageFilter), nameof(PageFilter));
            if (take > maxTake)
                take = maxTake;
            Skip = skip;
            Take = take;
            MaxTake = maxTake;
        }

        public bool Equals(PageFilter other)
        {
            return Skip == other.Skip && Take == other.Take;
        }

        public override bool Equals(object obj)
        {
            return obj is PageFilter other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(Skip, Take);

        public static bool operator ==(PageFilter lhs, PageFilter rhs) => lhs.Take == rhs.Take && lhs.Skip == rhs.Skip;

        public static bool operator !=(PageFilter lhs, PageFilter rhs) => !(lhs == rhs);
    }
}
