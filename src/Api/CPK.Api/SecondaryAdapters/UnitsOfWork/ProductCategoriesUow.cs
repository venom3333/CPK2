using CPK.ProductCategoriesModule.SecondaryPorts;
using CPK.ProductsModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal sealed class ProductCategoriesUow : UnitOfWorkBase, IProductCategoriesUow
    {
        public ProductCategoriesUow(CpkContext context, IProductCategoriesRepository repository) : base(context)
        {
            Repository = repository;
        }

        public IProductCategoriesRepository Repository { get; }
    }
}
