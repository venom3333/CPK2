using CPK.ProductCategoriesModule.SecondaryPorts;
using CPK.ProductsModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal sealed class ProductCategoriesUow : UnitOfWorkBase, IProductCategoriesUow
    {
        public ProductCategoriesUow(CpkContext context, IProductCategoriesRepository productCategories) : base(context)
        {
            ProductCategories = productCategories;
        }

        public IProductCategoriesRepository ProductCategories { get; }
    }
}
