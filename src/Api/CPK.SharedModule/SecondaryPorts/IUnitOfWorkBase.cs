using System.Threading.Tasks;

namespace CPK.SharedModule.SecondaryPorts
{
    public interface IUnitOfWorkBase
    {
        Task<int> SaveAsync();
    }
}
