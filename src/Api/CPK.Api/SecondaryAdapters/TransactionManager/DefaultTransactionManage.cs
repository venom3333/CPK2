using System.Threading.Tasks;
using CPK.SharedModule.SecondaryPorts;
using Microsoft.EntityFrameworkCore.Storage;

namespace CPK.Api.SecondaryAdapters.TransactionManager
{
    internal sealed class DefaultTransactionManage : ITransactionManager
    {
        private readonly IDbContextTransaction _transaction;

        public DefaultTransactionManage(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public Task Commit()
        {
            return _transaction.CommitAsync();
        }

        public Task RollBack()
        {
            return _transaction.RollbackAsync();
        }
    }
}
