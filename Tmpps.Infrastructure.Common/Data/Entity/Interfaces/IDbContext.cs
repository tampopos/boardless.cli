using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Tmpps.Infrastructure.Common.Data.Interfaces;

namespace Tmpps.Infrastructure.Common.Data.Entity.Interfaces
{
    public interface IDbContext : IDbTransactionManager
    {
        Task<int> SaveChangesAsync();
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        IDbQuery CreateDbQuery(string sql);
        IDbQuery CreateDbQueryById(string sqlId = null);
        Task<int> ExecuteAsync(IDbQuery query);
        Task<IEnumerable<T>> QueryAsync<T>(IDbQuery query);
        Task<T> QuerySingleOrDefaultAsync<T>(IDbQuery query);
        Task<T> QueryFirstOrDefaultAsync<T>(IDbQuery query);
    }
}