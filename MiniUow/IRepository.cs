//-----------------------------------------------------------------------
// <copyright file="IRepository.cs">
// Copyright (c) CanhHungIT. All rights reserved.
// https://www.nuget.org/packages/MiniUow/
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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

        ///// <summary>
        ///// Uses raw SQL queries to fetch the specified <typeparamref name="T"/> data.
        ///// </summary>
        ///// <typeparam name="T">The type of the entity.</typeparam>
        ///// <param name="sql">The raw SQL.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <returns>An <see cref="IQueryable{T}"/> that contains elements that satisfy the condition specified by raw SQL.</returns>
        //IQueryable<T> FromSql<T>(string sql, params object[] parameters) where T : class;

        /// <summary>
        /// Adds a new entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to Add.</param>
        T Add(T entity);

        /// <summary>
        /// Adds a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to Add.</param>
        void Add(params T[] entities);

        /// <summary>
        /// Adds a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to Add.</param>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Adds a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to Add.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous Add operation.</returns>
        Task AddAsync(params T[] entities);

        /// <summary>
        /// Adds a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to Add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous Add operation.</returns>
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