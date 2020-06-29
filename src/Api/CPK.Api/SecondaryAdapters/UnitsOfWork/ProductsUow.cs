using CPK.ProductsModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal sealed class ProductsUow : UnitOfWorkBase, IProductsUow
    {
        public ProductsUow(CpkContext context, IProductsRepository products) : base(context)
        {
            Products = products;
        }

        public IProductsRepository Products { get; }
    }
}
