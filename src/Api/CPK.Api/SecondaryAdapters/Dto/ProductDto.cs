using System;
using System.Collections.Generic;
using CPK.BasketModule.Entities;
using CPK.OrdersModule.Entities;
using CPK.ProductsModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class ProductDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid? ImageId { get; set; }
        
        public ProductCategoryDto Category { get; set; }
        public List<OrderLineDto> Orders { get; set; } = new List<OrderLineDto>();
        public List<BasketLineDto> Baskets { get; set; } = new List<BasketLineDto>();

        public ProductDto()
        {
        }

        public ProductDto(Guid id, string token, string title, decimal price, Guid? imageId)
        {
            Id = id;
            ConcurrencyToken = token;
            Title = title;
            Price = price;
            ImageId = imageId;
        }

        public ConcurrencyToken<Product> ToProduct() => new ConcurrencyToken<Product>(ConcurrencyToken,
            new Product(new Id(Id), new Title(Title), new Money(Price), new Image(ImageId)));

        public ConcurrencyToken<BasketProduct> ToBasketProduct() =>
            new ConcurrencyToken<BasketProduct>(ConcurrencyToken, new BasketProduct(Id, Title, Price));

        public ConcurrencyToken<OrderProduct> ToOrderProduct() =>
            new ConcurrencyToken<OrderProduct>(ConcurrencyToken, new OrderProduct(Id, Title, Price));

        public ProductDto(Product product, string version) : this(product.Id.Value, version, product.Title.Value,
            product.Price.Value, product.Image.Value)
        {
        }

        public ProductDto(OrderProduct product, string version)
        {
            Title = product.Title;
            Id = product.Id;
            Price = product.Price;
            ConcurrencyToken = version;
        }
    }
}