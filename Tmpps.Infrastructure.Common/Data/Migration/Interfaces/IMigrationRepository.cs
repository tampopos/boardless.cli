using System.Threading.Tasks;
using Tmpps.Infrastructure.Common.Data.Interfaces;

namespace Tmpps.Infrastructure.Common.Data.Migration.Interfaces
{
    public interface IMigrationRepository<TMigrationHistory> : IRepository<TMigrationHistory> where TMigrationHistory : class, IMigrationHistory
    {
        Task<bool> ExistsAsync(string id);
        Task<TMigrationHistory> GetByIdAsync(string id);
        Task AddAsync(TMigrationHistory entity);
    }
}