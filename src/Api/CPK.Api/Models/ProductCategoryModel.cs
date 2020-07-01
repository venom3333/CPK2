using System;

using CPK.BasketModule.Entities;
using CPK.OrdersModule.Entities;
using CPK.ProductCategoriesModule.Entities;
using CPK.ProductsModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.Api.Models
{
    public sealed class ProductCategoryModel
    {
        public Guid Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Guid ImageId { get; set; }

        public ProductCategoryModel()
        {

        }

        public ProductCategoryModel(ConcurrencyToken<ProductCategory> productCategory)
        {
            Id = productCategory.Entity.Id.Value;
            Version = productCategory.Token;
            Title = productCategory.Entity.Title.Value;
            ShortDescription = productCategory.Entity.ShortDescription.Value;
            ImageId = productCategory.Entity.Image.Id;
        }

        public ConcurrencyToken<ProductCategory> ToProductCategory() =>
            new ConcurrencyToken<ProductCategory>(Version, new ProductCategory(new Id(Id), new Title(Title), new ProductCategoryShortDescription(ShortDescription), new Image(ImageId)));
    }
}
