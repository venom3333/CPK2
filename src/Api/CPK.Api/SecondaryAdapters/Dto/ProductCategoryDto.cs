using System;
using System.Collections.Generic;

using CPK.BasketModule.Entities;
using CPK.OrdersModule.Entities;
using CPK.ProductCategoriesModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class ProductCategoryDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Guid ImageId { get; set; }

        public FileDto Image { get; set; }
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public ProductCategoryDto()
        {
            
        }
        public ProductCategoryDto(Guid id, string token, string title, string shortDescription, Guid imageId)
        {
            Id = id;
            ConcurrencyToken = token;
            Title = title;
            ShortDescription = shortDescription;
            ImageId = imageId;
        }

        public ConcurrencyToken<ProductCategory> ToProductCategory() =>
            new ConcurrencyToken<ProductCategory>(ConcurrencyToken, new ProductCategory(new Id(Id), new Title(Title), new ProductCategoryShortDescription(ShortDescription), new Image(ImageId)));

        public ProductCategoryDto(ProductCategory productCategory, string version) :
            this(productCategory.Id.Value, version, productCategory.Title.Value, productCategory.ShortDescription.Value, productCategory.Image.Value)
        {

        }
    }
}
