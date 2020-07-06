using System;

namespace CPK.Api.Models.ProductCategories
{
    public sealed class AddProductCategoryModel
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Guid ImageId { get; set; }
    }
}
