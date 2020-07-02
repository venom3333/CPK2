using System;

namespace CPK.Spa.Client.Core.Models
{
    public sealed class ProductCategoriesFilterModel
    {
        public Guid Id { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string Title { get; set; }

        public bool Descending { get; set; }
        public ProductCategoryOrderBy OrderBy { get; set; }
    }
}