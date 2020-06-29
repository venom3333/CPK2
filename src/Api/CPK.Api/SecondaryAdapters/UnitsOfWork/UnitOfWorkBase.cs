using System.Threading.Tasks;
using CPK.SharedModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal abstract class UnitOfWorkBase : IUnitOfWorkBase
    {
        private readonly CpkContext _context;

        protected UnitOfWorkBase(CpkContext context)
        {
            _context = context;
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
