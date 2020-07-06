using System;
using CPK.ProductCategoriesModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.Api.Models.ProductCategories
{
    public sealed class ProductCategoryModel
    {
        public Guid Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Guid? ImageId { get; set; }

        public ProductCategoryModel()
        {

        }

        public ProductCategoryModel(ConcurrencyToken<ProductCategory> productCategory)
        {
            Id = productCategory.Entity.Id.Value;
            Version = productCategory.Token;
            Title = productCategory.Entity.Title.Value;
            ShortDescription = productCategory.Entity.ShortDescription.Value;
            ImageId = productCategory.Entity.Image.Value;
        }

        public ConcurrencyToken<ProductCategory> ToProductCategory() =>
            new ConcurrencyToken<ProductCategory>(
                Version,
                new ProductCategory(
                    new Id(Id),
                    new Title(Title),
                    new ShortDescription(ShortDescription),
                    new Image(ImageId)));
    }
}
