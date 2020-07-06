using CPK.NewsModule.SecondaryPorts;
using CPK.ProductCategoriesModule.SecondaryPorts;
using CPK.ProductsModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal sealed class NewsUow : UnitOfWorkBase, INewsUow
    {
        public NewsUow(CpkContext context, INewsRepository repository) : base(context)
        {
            Repository = repository;
        }

        public INewsRepository Repository { get; }
    }
}
