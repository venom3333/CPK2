using CPK.SharedModule;
using CPK.SharedModule.Entities;

using FluentValidationGuard;

using System;

namespace CPK.ProductCategoriesModule.Dto
{
    public readonly struct ProductCategoriesFilter
    {
        public PageFilter PageFilter { get; }

        public Guid Id { get; }
        public string Title { get; }
        public ProductCategoryOrderBy OrderBy { get; }
        public bool Descending { get; }

        public ProductCategoriesFilter(PageFilter pageFilter, Guid id, string title, bool @descending, ProductCategoryOrderBy orderBy)
        {
            Validator.Begin(pageFilter, nameof(pageFilter))
                .NotDefault()
                .ThrowApiException(nameof(ProductCategoriesFilter), nameof(ProductCategoriesFilter));
            PageFilter = pageFilter;
            Title = title;
            Descending = @descending;
            OrderBy = orderBy;
            Id = id;
        }

        public bool Equals(ProductCategoriesFilter other)
        {
            return PageFilter.Equals(other.PageFilter) && string.Equals(Title, other.Title);
        }

        public override bool Equals(object obj)
        {
            return obj is ProductCategoriesFilter other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PageFilter.GetHashCode();
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Id != null ? Id.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ProductCategoriesFilter lhs, ProductCategoriesFilter rhs)
            => lhs.PageFilter == rhs.PageFilter &&
               string.Equals(lhs.Title, rhs.Title) &&
               lhs.Id.Equals(rhs.Id);
        public static bool operator !=(ProductCategoriesFilter lhs, ProductCategoriesFilter rhs) => !(lhs == rhs);

    }
}