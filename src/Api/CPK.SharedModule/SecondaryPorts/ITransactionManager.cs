using System;
using System.Threading.Tasks;

namespace CPK.SharedModule.SecondaryPorts
{
    public interface ITransactionManager : IDisposable
    {
        Task Commit();
        Task RollBack();
    }
}
