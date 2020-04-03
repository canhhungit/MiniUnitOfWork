//-----------------------------------------------------------------------
// <copyright file="IRepository.cs">
// Copyright (c) CanhHungIT. All rights reserved.
// https://www.nuget.org/packages/MiniUow/
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniUow
{
    public interface IRepository<T> : IReadRepository<T>, IDisposable where T : class
    {
        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// Inserts a new entity synchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities"></param>
        void Add(params T[] entities);

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities"></param>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities"></param>
        Task AddAsync(params T[] entities);

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities"></param>
        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities"></param>
        void Delete(params T[] entities);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities"></param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entities"></param>
        void Update(params T[] entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entities"></param>
        void Update(IEnumerable<T> entities);
    }
}