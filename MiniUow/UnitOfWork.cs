//-----------------------------------------------------------------------
// <copyright>
// Copyright (c) CanhHungIT. All rights reserved.
// https://www.nuget.org/packages/MiniUow/
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniUow
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork
        where TContext : DbContext, IDisposable
    {
        private Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            ThrowIfDisposed();
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }
            var type = typeof(TEntity);
            if (!_repositories.TryGetValue(type, out var repository))
            {
                repository = new Repository<TEntity>(Context);
                _repositories[type] = repository;
            }

            return (IRepository<TEntity>)repository;
        }

        public TContext Context { get; }

        public int SaveChanges()
        {
            ThrowIfDisposed();
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            ThrowIfDisposed();
            return await Context.SaveChangesAsync().ConfigureAwait(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
  	        GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // clear repositories
                _repositories?.Clear();

                // dispose the db context.
                Context.Dispose();
            }

            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
