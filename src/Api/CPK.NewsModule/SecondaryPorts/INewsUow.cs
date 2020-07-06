using CPK.SharedModule.SecondaryPorts;

namespace CPK.NewsModule.SecondaryPorts
{
    public interface INewsUow : IUnitOfWorkBase
    {
        public INewsRepository Repository { get; }
    }
}