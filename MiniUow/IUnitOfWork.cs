//-----------------------------------------------------------------------
// <copyright>
// Copyright (c) CanhHungIT. All rights reserved.
// https://www.nuget.org/packages/MiniUow/
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MiniUow
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the specified repository for the <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// Gets the db context.
        /// </summary>
        TContext Context { get; }
    }
}