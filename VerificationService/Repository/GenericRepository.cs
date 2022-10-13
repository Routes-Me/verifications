using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using VerificationService.Abstraction;
using VerificationService.Models.DBModel;
using System.Linq;

namespace VerificationService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly verificationsdatabaseContext context;
        internal DbSet<T> dbSet;

        public GenericRepository(verificationsdatabaseContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();

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
            T entity = await GetById(id);
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public T Where(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).FirstOrDefault();
        }
    }
}
