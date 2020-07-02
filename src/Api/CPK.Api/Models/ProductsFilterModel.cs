using CPK.ProductsModule.Dto;

namespace CPK.Api.Models
{
    public sealed class ProductsFilterModel
    {
        public uint Take { get; set; }
        public uint Skip { get; set; }
        public string Title { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool Descending { get; set; }
        public ProductOrderBy OrderBy { get; set; }
    }
}
