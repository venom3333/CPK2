using System;
using CPK.SharedModule;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.NewsModule.Dto
{
    public readonly struct NewsFilter
    {
        public PageFilter PageFilter { get; }

        public Guid Id { get; }
        public string Title { get; }
        public NewsOrderBy OrderBy { get; }
        public bool Descending { get; }

        public NewsFilter(PageFilter pageFilter, Guid id, string title, bool @descending, NewsOrderBy orderBy)
        {
            Validator.Begin(pageFilter, nameof(pageFilter))
                .NotDefault()
                .ThrowApiException(nameof(NewsFilter), nameof(NewsFilter));
            PageFilter = pageFilter;
            Title = title;
            Descending = @descending;
            OrderBy = orderBy;
            Id = id;
        }

        public bool Equals(NewsFilter other)
        {
            return PageFilter.Equals(other.PageFilter) && string.Equals(Title, other.Title);
        }

        public override bool Equals(object obj)
        {
            return obj is NewsFilter other && Equals(other);
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

        public static bool operator ==(NewsFilter lhs, NewsFilter rhs)
            => lhs.PageFilter == rhs.PageFilter &&
               string.Equals(lhs.Title, rhs.Title) &&
               lhs.Id.Equals(rhs.Id);
        public static bool operator !=(NewsFilter lhs, NewsFilter rhs) => !(lhs == rhs);

    }
}