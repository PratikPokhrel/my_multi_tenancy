using Core.ClientInfo;
using Core.EF.Data.Configuration.Pg;
using Core.EF.Data.Context;
using Core.EF.Data.Extensions;
using Core.Infrastructure.DataAccess;
using Core.Infrastructure.Tenancy;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Core.EF.Data.Configuration.Management
{
    /// <summary>
    /// The Entity Framework implementation of IUnitOfWork
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The context
        /// </summary>
        private IDbContext context;
        private readonly IClientInfoProvider _clientInfoProvider;
        private readonly IAppCache _cache;
        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        /// The repositories
        /// </summary>
        private Dictionary<Type, object> repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        public UnitOfWork(IContextFactory contextFactory, IClientInfoProvider clientInfoProvider,ITenantProvider tenantProvider, IAppCache cache)
        {
            context = contextFactory.DbContext;
            _clientInfoProvider = clientInfoProvider;
            _cache = cache;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            // Save changes with the default options
            context.ChangeTracker.DetectChanges();
            context.ChangeTracker.ProcessModification(_clientInfoProvider.UserId);
            context.ChangeTracker.ProcessDeletion(_clientInfoProvider.UserId);
            context.ChangeTracker.ProcessCreation(_clientInfoProvider.UserId,_tenantProvider.TenantId);
            return context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            // Save changes with the default options
            context.ChangeTracker.DetectChanges();
            context.ChangeTracker.ProcessModification(_clientInfoProvider.UserId);
            context.ChangeTracker.ProcessDeletion(_clientInfoProvider.UserId);
            context.ChangeTracker.ProcessCreation(_clientInfoProvider.UserId, _tenantProvider.TenantId);
            return await context.SaveChangesAsync().ConfigureAwait(false);
        }


        public async Task<int> CommitAndRemoveCacheAsync(params string[] cacheKeys)
        {
            var result= await CommitAsync().ConfigureAwait(false);
            foreach (var cacheKey in cacheKeys)
                _cache.Remove(cacheKey);
            return result;
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(obj: this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }

        public  async Task<T> ExecuteReaderAsync<T>(Func<DbDataReader, T> mapEntities, string exec, SqlParameter[] parameters = null) where T:class
        {
            OpenConnection();
            return await ExectAsync(mapEntities, exec, parameters).ConfigureAwait(false);
        }

        public void OpenConnection()
        {
            var dbContext = context as DeviceContext;

            if (dbContext.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                dbContext.Database.OpenConnection();
            }
        }
        private async Task<T> ExectAsync<T>(Func<DbDataReader, T> mapEntities,string exec, SqlParameter[] parameters = null)
        {
            var dbcOntext = context as DeviceContext;
            var command = dbcOntext.Database.GetDbConnection().CreateCommand();
            command.CommandText = exec;

            if (dbcOntext.Database.CurrentTransaction != null)
                command.Transaction = dbcOntext.Database.CurrentTransaction.GetDbTransaction();

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);


            //NOTE Removed this because data didnot bind in multiset object if 1st result set is null.
            //TODO But this could result in error.
            //if (!reader.HasRows)
            //    while (await reader.NextResultAsync()) { } 

            T data = mapEntities(reader);
            return data;
        }
    }
}
