namespace Tmpps.Infrastructure.Common.Data.Interfaces
{
    public interface IRepository<TKey, TEntity> where TEntity : class { }
    public interface IRepository<TEntity> : IRepository<string, TEntity> where TEntity : class { }
}