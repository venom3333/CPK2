using System.Collections.Generic;

namespace CPK.Spa.Client.Core.Models
{
    public sealed class PageResultModel<T>
    {
        public int TotalCount { get; set; }
        public List<T> Value { get; set; } = new List<T>();
    }
}