using System;

namespace CPK.Spa.Client.Core.Models.ProductCategories
{
    public sealed class ProductCategoryModel
    {
        public Guid Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Guid? ImageId { get; set; }
    }
}