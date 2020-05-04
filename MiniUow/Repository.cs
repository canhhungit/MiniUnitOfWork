//-----------------------------------------------------------------------
// <copyright>
// Copyright (c) CanhHungIT. All rights reserved.
// https://www.nuget.org/packages/MiniUow/
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MiniUow
{
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbContext context) : base(context)
        {
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters) => _dbContext.Database.ExecuteSqlCommand(sql, parameters);

        public virtual T Add(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public virtual void Add(params T[] entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var data = await _dbSet.AddAsync(entity, cancellationToken);
            return data.Entity;
        }

        public virtual Task AddAsync(params T[] entities) => _dbSet.AddRangeAsync(entities);

        public virtual Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken)) => _dbSet.AddRangeAsync(entities, cancellationToken);


        public virtual void Delete(T entity)
        {
            //var existing = _dbSet.Find(entity);
            //if (existing != null)
            _dbSet.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null) Delete(entity);
            }
        }

        public virtual void Delete(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }


        public virtual T Update(T entity)
        {
            return _dbSet.Update(entity).Entity;
        }

        public virtual void Update(params T[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }


        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}