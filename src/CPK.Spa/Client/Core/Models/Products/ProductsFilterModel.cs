namespace CPK.Spa.Client.Core.Models.Products
{
    public sealed class ProductsFilterModel
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Title { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool Descending { get; set; }
        public ProductOrderBy OrderBy { get; set; }
    }
}