using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.EF.Data.Context;
using Core.EF.Data.Extensions;
using Core.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Core.EF.Data.Configuration.Management
{
    /// <summary>
    /// Generic repository, contains CRUD operation of EF entity
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Repository<T> : IRepository<T>
        where T : class
    {
        /// <summary>
        /// EF data base context
        /// </summary>
        private readonly IDbContext context;

        /// <summary>
        /// Used to query and save instances of
        /// </summary>
        private readonly DbSet<T> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(IDbContext context)
        {
            this.context = context;

            dbSet = context.Set<T>();
        }

        /// <inheritdoc />
        public virtual T Add(T entity)
        {
            var res= dbSet.Add(entity);
            return res.Entity;
        }


        /// <inheritdoc />
        public virtual IQueryable<T> TableNoTraking
        {
            get
            {
                return dbSet.AsNoTracking();
            }
        }

        /// <inheritdoc />
        public virtual IQueryable<T> Table
        {
            get
            {
                return dbSet;
            }
        }

        /// <inheritdoc />
        public T Get<TKey>(TKey id)
        {
            return dbSet.Find(id);
        }

        /// <inheritdoc/>
        public T Get<TKey, TProperty>(TKey id, Expression<Func<T, TProperty>> navigationPropertyPath) where TProperty : class
        {
            var entity = dbSet.Find(id);

            if (entity == null)
                return null;
            context.Entry(entity).Reference(navigationPropertyPath).Load();
            return entity;
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<TKey, TProperty>(TKey id, Expression<Func<T, TProperty>> navigationPropertyPath) where TProperty : class
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null) return null;
            await context.Entry(entity).Reference(navigationPropertyPath).LoadAsync();
            return entity;
        }

        /// <inheritdoc />
        public T Get(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        /// <inheritdoc />
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        /// <inheritdoc />
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include)
        {
            return FindBy(predicate).Include(include);
        }

        /// <inheritdoc />
        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        /// <inheritdoc />
        public IQueryable<T> GetAll(int page, int pageCount)
        {
            var pageSize = (page - 1) * pageCount;

            return dbSet.Skip(pageSize).Take(pageCount);
        }

        /// <inheritdoc />
        public IQueryable<T> GetAll<TProperty>(int page, int pageCount, Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            var pageSize = (page - 1) * pageCount;

            return dbSet.Include(navigationPropertyPath).Skip(pageSize).Take(pageCount);
        }

        /// <inheritdoc />
        public IQueryable<T> GetAll(string include)
        {
            return dbSet.Include(include);
        }

        /// <inheritdoc />

        /// <inheritdoc />
        public IQueryable<T> GetAll(string include, string include2)
        {
            return dbSet.Include(include).Include(include2);
        }

        /// <inheritdoc />
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }

        /// <inheritdoc />


        /// <inheritdoc />
        public T Delete(T entity)
        {
             dbSet.Remove(entity);
            return entity;
        }

        /// <inheritdoc />
        public virtual T Update(T entity)
        {
             dbSet.Update(entity);
            return entity as T;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync().ConfigureAwait(false);
        }
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="select"></param>
        /// <param name="asNoTracking"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<List<TType>> GetAllAsync<TType>(Expression<Func<T, TType>> select, bool asNoTracking = false, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(select).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public IQueryable<T> FromSql(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters);
        }

        public async Task<List<T>> IncludeAsync(IQueryable<T> query, params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var include in includeProperties)
                query = query.Include(include);

            return await query.ToListAsync().ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public virtual void UpdateRange(List<T> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return dbSet.AsNoTracking();
            }
        }

        /// <inheritdoc />
        public async Task<List<TType>> GetAllAsync<TType>(Expression<Func<T, bool>> where, Expression<Func<T, TType>> select, bool asNoTracking = false, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(select).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="asNoTracking"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, bool asNoTracking = false, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = dbSet;

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }


        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = filter != null ? dbSet.Where(filter) : dbSet;
            return await query.CountAsync().ConfigureAwait(false);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await dbSet.AddRangeAsync(entities).ConfigureAwait(false);
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity).ConfigureAwait(false);
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null)
        {
            if(filter != null)
                return await dbSet.AnyAsync(filter).ConfigureAwait(false);
            else
                return await dbSet.AnyAsync();
        }

        public async Task<T> GetAsyncTemp(Expression<Func<T, bool>> predicate, params string[] includesStr)
        {
            Func<IQueryable<T>, IQueryable<T>> includes = includesStr == null || !includesStr.Any() ? null : DbContextHelper.GetNavigations<T>(includesStr.ToList());
            IQueryable<T> query = dbSet;
            if (includes != null)
            {
                query = includes(query);
            }

            var entity = await query.FirstOrDefaultAsync(predicate);
            return entity;
        }

        public async Task<List<T>> GetAsyncTempList(Expression<Func<T, bool>> predicate,params string[] includesStr)
        {
            Func<IQueryable<T>, IQueryable<T>> includes =includesStr==null || !includesStr.Any()?null: DbContextHelper.GetNavigations<T>(includesStr.ToList());
            IQueryable<T> query = dbSet;
            if (includes != null)
            {
                query = includes(query);
            }

            var entity = await query.Where(predicate).ToListAsync();
            return entity;
        }
    }
}
