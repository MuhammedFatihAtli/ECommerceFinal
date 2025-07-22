using ECommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : IBase
    {
        Task<T?> GetByIdAsync(int id, bool ignoreFilters = false);
        Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>> filter = null,
    bool isTrack = true,
    bool ignoreFilters = false,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<IEnumerable<T>> FindConditionAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false);

        Task<IEnumerable<TResult>> GetFilteredListAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);

        Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> condition);
        Task<int> CountAsync(Expression<Func<T, bool>> condition = null);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void SoftDelete(T item);
        Task AddAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    }
}
