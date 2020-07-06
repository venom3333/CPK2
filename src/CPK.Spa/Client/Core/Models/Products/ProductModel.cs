using System;

namespace CPK.Spa.Client.Core.Models.Products
{
    public sealed class ProductModel
    {
        public Guid Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid ImageId { get; set; }
    }
}