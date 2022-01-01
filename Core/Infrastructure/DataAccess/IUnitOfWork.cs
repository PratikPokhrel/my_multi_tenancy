using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.DataAccess
{
    /// <summary>
    /// Contains functions to manipulate EF transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Repository</returns>
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();

        /// <summary>
        /// Saves all pending changes Asynchronously
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Commit and remove cache
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="cacheKeys"></param>
        /// <returns></returns>
        Task<int> CommitAndRemoveCacheAsync(params string[] cacheKeys);

        Task<T> ExecuteReaderAsync<T>(Func<DbDataReader, T> mapEntities, string exec, SqlParameter[] parameters = null) where T : class;
    }

    public interface IDefaultUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Repository</returns>
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();
    }
}