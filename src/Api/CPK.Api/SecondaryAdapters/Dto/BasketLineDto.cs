using System;
using CPK.BasketModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class BasketLineDto
    {
        public BasketDto Basket { get; set; }

        public string BasketId { get; set; }
        public ProductDto Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public BasketLineDto(BasketDto basket, ProductDto product, int quantity)
        {
            Basket = basket;
            Product = product;
            BasketId = basket.Id;
            ProductId = product.Id;
            Quantity = quantity;
        }

        public BasketLineDto()
        {
            //For EF
        }

        public BasketLineDto(BasketLine line, BasketDto basket)
        {
            Basket = basket;
            BasketId = basket.Id;
            ProductId = line.Product.Id;
            Quantity = (int)line.Quantity;
        }

        public BasketLine ToLine() => new BasketLine(Product.ToBasketProduct().Entity, (uint)Quantity);
    }
}
