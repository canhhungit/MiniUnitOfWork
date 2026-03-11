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
using System.Linq;
using System.Linq.Dynamic.Core;
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
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        protected virtual IQueryable<T> BuildQuery(
            Expression<Func<T, bool>> predicate = null,
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

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        protected virtual IQueryable<T> BuildQuery(
            string predicate = null,
            string orderBy = null,
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

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (!string.IsNullOrWhiteSpace(predicate))
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderBy);
            }

            return query;
        }

        #region Exists

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
            return await AnyAsync(selector).ConfigureAwait(false);
        }

        #endregion Exists

        #region Count

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
                return await _dbSet.CountAsync().ConfigureAwait(false);
            }
            else
            {
                return await _dbSet.CountAsync(predicate).ConfigureAwait(false);
            }
        }

        #endregion Count

        #region LongCount

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
                return await _dbSet.LongCountAsync().ConfigureAwait(false);
            }
            else
            {
                return await _dbSet.LongCountAsync(predicate).ConfigureAwait(false);
            }
        }

        #endregion LongCount

        #region Max

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
                return await _dbSet.MaxAsync(selector).ConfigureAwait(false);
            }
            else
            {
                return await _dbSet.Where(predicate).MaxAsync(selector).ConfigureAwait(false);
            }
        }

        #endregion Max

        #region Min

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
                return await _dbSet.MinAsync(selector).ConfigureAwait(false);
            }
            else
            {
                return await _dbSet.Where(predicate).MinAsync(selector).ConfigureAwait(false);
            }
        }

        #endregion Min

        #region Average

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
                return await _dbSet.AverageAsync(selector).ConfigureAwait(false);
            }
            else
            {
                return await _dbSet.Where(predicate).AverageAsync(selector).ConfigureAwait(false);
            }
        }

        #endregion Average

        #region Sum

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
                return await _dbSet.SumAsync(selector).ConfigureAwait(false);
            }
            else
            {
                return await _dbSet.Where(predicate).SumAsync(selector).ConfigureAwait(false);
            }
        }

        #endregion Sum

        public virtual IQueryable<T> Query(string sql, params object[] parameters) => _dbSet.FromSqlRaw(sql, parameters);

        #region FindAsync

        public virtual T Find(params object[] keyValues) => _dbSet.Find(keyValues);

        public virtual async Task<T> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues).AsTask().ConfigureAwait(false);

        public virtual Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken) => _dbSet.FindAsync(keyValues, cancellationToken).AsTask();

        #endregion FindAsync

        #region Single

        public virtual T Single(Expression<Func<T, bool>> predicate = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
          bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return query.SingleOrDefault();
        }

        public virtual async Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return await query.SingleOrDefaultAsync().ConfigureAwait(false);
        }

        #endregion Single

        #region GetPagedList

        public virtual IPaginate<T> GetPagedList(Expression<Func<T, bool>> predicate = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
          int size = 20, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).ToPaginate(index, size);
        }

        public virtual IPaginate<T> GetPagedList(string predicate = null,
          string orderBy = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
          int size = 20, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).ToPaginate(index, size);
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
            return await BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters)
                .ToPaginateAsync(index, size, 0, cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual async Task<IPaginate<T>> GetPagedListAsync(string predicate = null,
           string orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
           int index = 0,
           int size = 20,
           bool disableTracking = true,
           CancellationToken cancellationToken = default(CancellationToken),
           bool ignoreQueryFilters = false)
        {
            return await BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters)
                .ToPaginateAsync(index, size, 0, cancellationToken)
                .ConfigureAwait(false);
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
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).Select(selector).ToPaginate(index, size);
        }

        public virtual IPaginate<TResult> GetPagedList<TResult>(Expression<Func<T, TResult>> selector,
           string predicate = null,
           string orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
           int index = 0,
           int size = 20,
           bool disableTracking = true,
           bool ignoreQueryFilters = false) where TResult : class
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).Select(selector).ToPaginate(index, size);
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
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return await query.Select(selector).ToPaginateAsync(pageIndex, pageSize, 0, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<T, TResult>> selector,
          string predicate = null,
          string orderBy = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
          int pageIndex = 0,
          int pageSize = 20,
          bool disableTracking = true,
          CancellationToken cancellationToken = default,
          bool ignoreQueryFilters = false) where TResult : class
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return await query.Select(selector).ToPaginateAsync(pageIndex, pageSize, 0, cancellationToken).ConfigureAwait(false);
        }

        #endregion GetPagedList

        #region GetAll

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
         bool disableTracking = true,
         bool ignoreQueryFilters = false)
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
        }

        public virtual IQueryable<T> GetAll(string predicate = null,
        string orderBy = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
         bool disableTracking = true,
         bool ignoreQueryFilters = false)
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
        }

        public virtual IQueryable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
           bool disableTracking = true,
           bool ignoreQueryFilters = false) where TResult : class
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return query.Select(selector);
        }

        public virtual IQueryable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector,
            string predicate = null,
            string orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false) where TResult : class
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return query.Select(selector);
        }

        public virtual Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return Task.FromResult(query);
        }

        public virtual Task<IQueryable<T>> GetAllAsync(string predicate = null,
            string orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return Task.FromResult(query);
        }

        public virtual Task<IQueryable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false) where TResult : class
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return Task.FromResult(query.Select(selector));
        }

        public virtual Task<IQueryable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, string predicate = null,
         string orderBy = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
         bool disableTracking = true,
         bool ignoreQueryFilters = false) where TResult : class
        {
            var query = BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
            return Task.FromResult(query.Select(selector));
        }

        //public virtual IQueryable<T> GetAll()
        //{
        //    return _dbSet;
        //}

        //public async Task<IList<T>> GetAllAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}

        #endregion GetAll

        #region Any

        public virtual bool Any(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? _dbSet.Any() : _dbSet.Any(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.AnyAsync().ConfigureAwait(false);
            }

            return await _dbSet.AnyAsync(predicate).ConfigureAwait(false);
        }

        #endregion Any

        #region FirstOrDefault

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).FirstOrDefault();
        }

        public virtual T FirstOrDefault(string predicate = null,
          string orderBy = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
          bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).FirstOrDefault();
        }

        public virtual TResult FirstOrDefault<TResult>(Expression<Func<T, TResult>> selector,
                                                 Expression<Func<T, bool>> predicate = null,
                                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                 Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                 bool disableTracking = true,
                                                 bool ignoreQueryFilters = false) where TResult : class
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).Select(selector).FirstOrDefault();
        }

        public virtual TResult FirstOrDefault<TResult>(Expression<Func<T, TResult>> selector,
                                               string predicate = null,
                                               string orderBy = null,
                                               Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                               bool disableTracking = true,
                                               bool ignoreQueryFilters = false) where TResult : class
        {
            return BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).Select(selector).FirstOrDefault();
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            return await BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> FirstOrDefaultAsync(string predicate = null,
            string orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            return await BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public virtual async Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector,
                                              Expression<Func<T, bool>> predicate = null,
                                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                              bool disableTracking = true,
                                              bool ignoreQueryFilters = false) where TResult : class
        {
            return await BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).Select(selector).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public virtual async Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector,
                                            string predicate = null,
                                            string orderBy = null,
                                            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                            bool disableTracking = true,
                                            bool ignoreQueryFilters = false) where TResult : class
        {
            return await BuildQuery(predicate, orderBy, include, disableTracking, ignoreQueryFilters).Select(selector).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        #endregion FirstOrDefault
    }
}
