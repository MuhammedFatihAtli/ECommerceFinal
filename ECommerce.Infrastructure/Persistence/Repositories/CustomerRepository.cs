using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // CustomerRepository sınıfı, Customer entity'si için veritabanı işlemlerini gerçekleştirir.
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Add(Customer item)
        {
            _context.Customers.Add(item);
        }

        public async Task AddAsync(Customer customer) => await _context.Customers.AddAsync(customer);

        public async Task<bool> AnyAsync(Expression<Func<Customer, bool>> condition)
        {
            return await _context.Customers.AnyAsync(condition);
        }

        public async Task<int> CountAsync(Expression<Func<Customer, bool>> condition = null)
        {
            return condition == null
                ? await _context.Customers.CountAsync()
                : await _context.Customers.CountAsync(condition);
        }

        public void Delete(Customer item)
        {
            _context.Customers.Remove(item);
        }

        public void SoftDelete(Customer item)
        {
            item.IsDeleted = true;
            _context.Customers.Update(item);
        }

        public void Update(Customer item)
        {
            _context.Customers.Update(item);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<Customer> query = _context.Customers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id, bool ignoreFilters = false)
        {
            IQueryable<Customer> query = _context.Customers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> FirstOrDefaultAsync(Expression<Func<Customer, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<Customer> query = _context.Customers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(condition);
        }

        public async Task<IEnumerable<Customer>> FindConditionAsync(Expression<Func<Customer, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<Customer> query = _context.Customers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.Where(condition).ToListAsync();
        }

        public async Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(
            Expression<Func<Customer, TResult>> select,
            Expression<Func<Customer, bool>> where = null,
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null,
            Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>> join = null)
        {
            IQueryable<Customer> query = _context.Customers;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(select).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TResult>> GetFilteredListAsync<TResult>(
            Expression<Func<Customer, TResult>> select,
            Expression<Func<Customer, bool>> where = null,
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null,
            Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>> join = null)
        {
            IQueryable<Customer> query = _context.Customers;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(select).ToListAsync();
        }
    }
}

