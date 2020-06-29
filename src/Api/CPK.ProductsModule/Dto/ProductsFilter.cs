using CPK.SharedModule;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.ProductsModule.Dto
{
    public readonly struct ProductsFilter
    {
        public PageFilter PageFilter { get; }
        public string Title { get; }
        public decimal MinPrice { get; }
        public decimal MaxPrice { get; }
        public ProductOrderBy OrderBy { get; }
        public bool Descending { get; }

        public ProductsFilter(PageFilter pageFilter, string title, decimal minPrice, decimal maxPrice, bool @descending, ProductOrderBy orderBy)
        {
            Validator.Begin(pageFilter, nameof(pageFilter))
                .NotDefault()
                .Map(minPrice, nameof(minPrice))
                .IsGreaterOrEquals(0)
                .Map(maxPrice, nameof(maxPrice))
                .IsGreaterOrEquals(minPrice)
                .ThrowApiException(nameof(ProductsFilter), nameof(ProductsFilter));
            PageFilter = pageFilter;
            Title = title;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            Descending = @descending;
            OrderBy = orderBy;
        }


        public bool Equals(ProductsFilter other)
        {
            return PageFilter.Equals(other.PageFilter) && string.Equals(Title, other.Title) && MinPrice == other.MinPrice && MaxPrice == other.MaxPrice;
        }

        public override bool Equals(object obj)
        {
            return obj is ProductsFilter other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PageFilter.GetHashCode();
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ MinPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxPrice.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ProductsFilter lhs, ProductsFilter rhs)
            => lhs.PageFilter == rhs.PageFilter &&
               string.Equals(lhs.Title, rhs.Title) &&
               lhs.MinPrice == rhs.MinPrice &&
               lhs.MaxPrice == rhs.MaxPrice;
        public static bool operator !=(ProductsFilter lhs, ProductsFilter rhs) => !(lhs == rhs);

    }
}
