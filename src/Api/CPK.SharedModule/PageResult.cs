using System.Collections.Generic;
using System.Linq;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.SharedModule
{
    public readonly struct PageResult<T>
    {
        private readonly List<T> _value;
        public PageFilter PageFilter { get; }
        public uint TotalCount { get; }
        public IReadOnlyCollection<T> Value => _value.AsReadOnly();
        public PageResult(PageFilter pageFilter, IEnumerable<T> value, uint totalCount)
        {
            _value = value?.ToList();
            PageFilter = pageFilter;
            TotalCount = totalCount;
            Validator
                .Begin(_value, nameof(_value))
                .NotNull()
                .Map(PageFilter, nameof(PageFilter))
                .NotDefault()
                .ThrowApiException(nameof(PageResult<T>), nameof(PageResult<T>));
        }
    }
}
