using System;

using CPK.BasketModule.Entities;
using CPK.OrdersModule.Entities;
using CPK.ProductsModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.Api.Models
{
    public sealed class ProductModel
    {
        public Guid Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid ImageId { get; set; }

        public ProductModel()
        {

        }

        public ProductModel(ConcurrencyToken<Product> product)
        {
            Id = product.Entity.Id.Value;
            Version = product.Token;
            Title = product.Entity.Title.Value;
            Price = product.Entity.Price.Value;
            ImageId = product.Entity.Image.Id;
        }

        public ConcurrencyToken<Product> ToProduct() => new ConcurrencyToken<Product>(Version, new Product(new Id(Id), new Title(Title), new Money(Price), new Image(ImageId)));
        public BasketProduct ToBasketProduct() => new BasketProduct(Id, Title, Price);
        public OrderProduct ToOrderProduct() => new OrderProduct(Id, Title, Price);
    }
}
