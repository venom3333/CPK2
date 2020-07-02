using CPK.ProductCategoriesModule.Dto;
using CPK.ProductsModule.Dto;

using System;

namespace CPK.Api.Models
{
    public sealed class ProductCategoriesFilterModel
    {
        public Guid Id { get; set; }
        public uint Take { get; set; }
        public uint Skip { get; set; }
        public string Title { get; set; }
        
        public bool Descending { get; set; }
        public ProductCategoryOrderBy OrderBy { get; set; }
    }
}
