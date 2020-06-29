using System.Collections.Generic;
using CPK.SharedModule;

namespace CPK.Api.Models
{
    public sealed class PageResultModel<T>
    {
        public PageFilter ProductsFilter { get; set; }
        public uint TotalCount { get; set; }
        public List<T> Value { get; set; }
    }
}
