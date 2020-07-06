using CPK.SharedModule.SecondaryPorts;

namespace CPK.ProductCategoriesModule.SecondaryPorts
{
    public interface IProductCategoriesUow : IUnitOfWorkBase
    {
        public IProductCategoriesRepository Repository { get; }
    }
}