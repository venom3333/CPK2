using CPK.ProductCategoriesModule.Dto;
using CPK.ProductsModule.Dto;

namespace CPK.Api.Models
{
    public sealed class ProductCategoriesPageModel
    {
        public uint Take { get; set; }
        public uint Skip { get; set; }
        public string Title { get; set; }
        
        public bool Descending { get; set; }
        public ProductCategoryOrderBy OrderBy { get; set; }
    }
}
