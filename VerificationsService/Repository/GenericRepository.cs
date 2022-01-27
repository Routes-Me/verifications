using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VerificationsService.Models.DBModel;
using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;

namespace VerificationsService.Service
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Delete(int id);
        Task<bool> Upsert(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected verificationsdatabaseContext context;
        internal DbSet<T> dbSet;
        // protected readonly ILogger _logger;, ILogger logger

        public GenericRepository(verificationsdatabaseContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            // this._logger = logger;

        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }



        public virtual async Task<T> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Delete(int id)
        {
            T entity  = await GetById(id);
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return entity; 
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
    }
    

}