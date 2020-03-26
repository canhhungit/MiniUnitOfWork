using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MiniUow
{
    public interface IRepository<T> : IReadRepository<T>, IDisposable where T : class
    {
        int ExecuteSqlCommand(string sql, params object[] parameters);

        T Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));
        Task AddAsync(params T[] entities);
        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        void Delete(T entity);
        void Delete(object id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);


        T Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);

    }
}