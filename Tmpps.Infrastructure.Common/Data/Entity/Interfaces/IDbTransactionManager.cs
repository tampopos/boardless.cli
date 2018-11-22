using System.Data;
using System.Threading.Tasks;
using Tmpps.Infrastructure.Common.Data.Interfaces;

namespace Tmpps.Infrastructure.Common.Data.Entity.Interfaces
{
    public interface IDbTransactionManager
    {
        Task<ITransaction> BeginTransactionAsync();
    }
}