using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // SellerRepository sınıfı, Seller entity'si için veritabanı işlemlerini gerçekleştirir.
    public class SellerRepository :GenericRepository<Seller>, ISellerRepository
    {
        private readonly AppDbContext _context;

        public SellerRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public void Add(Seller item)
        {
            _context.Sellers.Add(item);
        }

        public async Task AddAsync(Seller seller) => await _context.Sellers.AddAsync(seller);

        public async Task<bool> AnyAsync(Expression<Func<Seller, bool>> condition)
        {
            return await _context.Sellers.AnyAsync(condition);
        }

        public async  Task<int> CountAsync(Expression<Func<Seller, bool>> condition = null)
        {
            return condition == null
        ? await _context.Sellers.CountAsync()
        : await _context.Sellers.CountAsync(condition);
        }

        public void Delete(Seller item)
        {
            _context.Sellers.Remove(item);
        }

        public async Task<IEnumerable<Seller>> FindConditionAsync(Expression<Func<Seller, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<Seller> query = _context.Sellers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.Where(condition).ToListAsync();
        }

        public async Task<Seller?> FirstOrDefaultAsync(Expression<Func<Seller, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<Seller> query = _context.Sellers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(condition);
        }

        public async Task<IEnumerable<Seller>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<Seller> query = _context.Sellers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<Seller?> GetByIdAsync(int id, bool ignoreFilters = false)
        {
            IQueryable<Seller> query = _context.Sellers;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(Expression<Func<Seller, TResult>> select, Expression<Func<Seller, bool>> where = null, Func<IQueryable<Seller>, IOrderedQueryable<Seller>> orderBy = null, Func<IQueryable<Seller>, IIncludableQueryable<Seller, object>> join = null)
        {
            IQueryable<Seller> query = _context.Sellers;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(select).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TResult>> GetFilteredListAsync<TResult>(Expression<Func<Seller, TResult>> select, Expression<Func<Seller, bool>> where = null, Func<IQueryable<Seller>, IOrderedQueryable<Seller>> orderBy = null, Func<IQueryable<Seller>, IIncludableQueryable<Seller, object>> join = null)
        {
            IQueryable<Seller> query = _context.Sellers;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(select).ToListAsync();
        }

        public void SoftDelete(Seller item)
        {
            item.IsDeleted = true;
            _context.Sellers.Update(item);
        }

        public void Update(Seller item)
        {
            _context.Sellers.Update(item);
        }
        
    }
}
