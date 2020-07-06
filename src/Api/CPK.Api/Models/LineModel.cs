using CPK.Api.Models.Products;
using CPK.BasketModule.Entities;
using CPK.OrdersModule.Entities;

namespace CPK.Api.Models
{
    public sealed class LineModel
    {
        public ProductModel Product { get; set; }
        public uint Quantity { get; set; }

        public LineModel()
        {
            //For framework
        }

        public LineModel(BasketLine line)
        {
            Product = new ProductModel
            {
                Id = line.Product.Id,
                Version = null,
                Title = line.Product.Title,
                Price = line.Product.Price
            };
            Quantity = line.Quantity;
        }

        public LineModel(OrderLine line)
        {
            Product = new ProductModel
            {
                Id = line.Product.Id,
                Version = null,
                Title = line.Product.Title,
                Price = line.Product.Price
            };
            Quantity = line.Quantity;
        }

        public BasketLine ToBasketLine() => new BasketLine(Product.ToBasketProduct(), Quantity);
        public OrderLine ToOrderLine() => new OrderLine(Product.ToOrderProduct(), Quantity);
    }
}
