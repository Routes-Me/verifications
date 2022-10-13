using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace VerificationService.Abstraction
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Delete(int id);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        T Where(Expression<Func<T, bool>> predicate);
    }
}
