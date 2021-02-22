//-----------------------------------------------------------------------
// <copyright>
// Copyright (c) CanhHungIT. All rights reserved.
// https://www.nuget.org/packages/MiniUow/
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MiniUow.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MiniUow
{
    public abstract class BaseRepository<T> : IReadRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual bool Exists(Expression<Func<T, bool>> selector = null)
        {
            if (selector == null)
            {
                return _dbSet.Any();
            }
            else
            {
                return _dbSet.Any(selector);
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> selector = null)
        {
            if (selector == null)
            {
                return await _dbSet.AnyAsync();
            }
            else
            {
                return await _dbSet.AnyAsync(selector);
            }
        }

        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.Count();
            }
            else
            {
                return _dbSet.Count(predicate);
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            else
            {
                return await _dbSet.CountAsync(predicate);
            }
        }

        public virtual long LongCount(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.LongCount();
            }
            else
            {
                return _dbSet.LongCount(predicate);
            }
        }

        public virtual async Task<long> LongCountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.LongCountAsync();
            }
            else
            {
                return await _dbSet.LongCountAsync(predicate);
            }
        }

        public virtual TEntity Max<TEntity>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TEntity>> selector = null)
        {
            if (predicate == null)
            {
                return _dbSet.Max(selector);
            }
            else
            {
                return _dbSet.Where(predicate).Max(selector);
            }
        }

        public virtual async Task<TEntity> MaxAsync<TEntity>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TEntity>> selector = null)
        {
            if (predicate == null)
            {
                return await _dbSet.MaxAsync(selector);
            }
            else
            {
                return await _dbSet.Where(predicate).MaxAsync(selector);
            }
        }

        public virtual TEntity Min<TEntity>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TEntity>> selector = null)
        {
            if (predicate == null)
            {
                return _dbSet.Min(selector);
            }
            else
            {
                return _dbSet.Where(predicate).Min(selector);
            }
        }

        public virtual async Task<TEntity> MinAsync<TEntity>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TEntity>> selector = null)
        {
            if (predicate == null)
            {
                return await _dbSet.MinAsync(selector);
            }
            else
            {
                return await _dbSet.Where(predicate).MinAsync(selector);
            }
        }

        public virtual decimal Average(Expression<Func<T, bool>> predicate = null, Expression<Func<T, decimal>> selector = null)
        {
            if (predicate == null)
            {
                return _dbSet.Average(selector);
            }
            else
            {
                return _dbSet.Where(predicate).Average(selector);
            }
        }

        public virtual async Task<decimal> AverageAsync(Expression<Func<T, bool>> predicate = null, Expression<Func<T, decimal>> selector = null)
        {
            if (predicate == null)
            {
                return await _dbSet.AverageAsync(selector);
            }
            else
            {
                return await _dbSet.Where(predicate).AverageAsync(selector);
            }
        }

        public virtual decimal Sum(Expression<Func<T, bool>> predicate = null, Expression<Func<T, decimal>> selector = null)
        {
            if (predicate == null)
            {
                return _dbSet.Sum(selector);
            }
            else
            {
                return _dbSet.Where(predicate).Sum(selector);
            }
        }

        public virtual async Task<decimal> SumAsync(Expression<Func<T, bool>> predicate = null, Expression<Func<T, decimal>> selector = null)
        {
            if (predicate == null)
            {
                return await _dbSet.SumAsync(selector);
            }
            else
            {
                return await _dbSet.Where(predicate).SumAsync(selector);
            }
        }

        public virtual IQueryable<T> Query(string sql, params object[] parameters) => _dbSet.FromSql(sql, parameters);

        public virtual T Find(params object[] keyValues) => _dbSet.Find(keyValues);

        public virtual async Task<T> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);

        public virtual Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken) => _dbSet.FindAsync(keyValues, cancellationToken);


        public virtual T Single(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }

            return query.FirstOrDefault();
        }

        public virtual async Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }
            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual IPaginate<T> GetPagedList(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
            int size = 20, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }

        public virtual async Task<IPaginate<T>> GetPagedListAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true,
            CancellationToken cancellationToken = default(CancellationToken),
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);
            }

            return await query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public virtual IPaginate<TResult> GetPagedList<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true,
            bool ignoreQueryFilters = false) where TResult : class
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return orderBy != null
                ? orderBy(query).Select(selector).ToPaginate(index, size)
                : query.Select(selector).ToPaginate(index, size);
        }

        public virtual async Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int pageIndex = 0,
            int pageSize = 20,
            bool disableTracking = true,
            CancellationToken cancellationToken = default,
            bool ignoreQueryFilters = false) where TResult : class
        {

            IQueryable<T> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).ToPaginateAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return await query.Select(selector).ToPaginateAsync(pageIndex, pageSize, 0, cancellationToken);
            }

        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return orderBy != null ? orderBy(query) : query;
        }

        public virtual IQueryable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
           bool disableTracking = true,
           bool ignoreQueryFilters = false) where TResult : class
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return orderBy != null ? orderBy(query).Select(selector) : query.Select(selector);
        }

        public virtual async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (orderBy != null)
            {
                return await Task.Run(() => orderBy(query));
            }

            return await Task.Run(() => query);
        }

        public virtual async Task<IQueryable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false) where TResult : class
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (orderBy != null)
            {
                return await Task.Run(() => orderBy(query).Select(selector));
            }

            return await Task.Run(() => query.Select(selector));
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate = null)
        {
            return _dbSet.Any(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            return Single(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
        }

        public virtual TResult FirstOrDefault<TResult>(Expression<Func<T, TResult>> selector,
                                                 Expression<Func<T, bool>> predicate = null,
                                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                 Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                 bool disableTracking = true,
                                                 bool ignoreQueryFilters = false) where TResult : class
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).FirstOrDefault();
            }
            else
            {
                return query.Select(selector).FirstOrDefault();
            }
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            return await SingleAsync(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
        }

        public virtual async Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector,
                                              Expression<Func<T, bool>> predicate = null,
                                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                              bool disableTracking = true,
                                              bool ignoreQueryFilters = false) where TResult : class
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).FirstOrDefaultAsync();
            }
            else
            {
                return await query.Select(selector).FirstOrDefaultAsync();
            }
        }
    }
}