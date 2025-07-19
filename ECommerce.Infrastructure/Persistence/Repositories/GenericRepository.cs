using ECommerce.Domain.Commons;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // Generic Repository (Genel Depo) sınıfı – EF Core ile çalışmak için
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBase
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        // Constructor – Veritabanı bağlamını alır ve ilgili DbSet'i tanımlar
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Yeni bir nesne ekler
        public void Add(T item)
        {
            _dbSet.Add(item);
        }

        // Belirtilen şartı sağlayan herhangi bir veri var mı kontrol eder
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.AnyAsync(condition);
        }

        // Şartlı ya da şartsız kayıt sayısını getirir
        public Task<int> CountAsync(Expression<Func<T, bool>> condition = null)
        {
            if (condition == null)
                return _dbSet.CountAsync();

            return _dbSet.CountAsync(condition);
        }

        // Bir nesneyi siler
        public void Delete(T item)
        {
            _dbSet.Remove(item);
        }

        // Belirli bir şartla eşleşen kayıtları getirir (takipli veya takipsiz, filtreli veya filtresiz)
        public async Task<IEnumerable<T>> FindConditionAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.Where(condition).ToListAsync();
        }

        // Şartla eşleşen ilk kaydı getirir (opsiyonel olarak takip/filtresiz)
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(condition);
        }

        // Tüm kayıtları getirir (isteğe bağlı olarak takip ve filtre kapatılabilir)
        public async Task<IEnumerable<T>> GetAllAsync(
      Expression<Func<T, bool>> filter = null,
      bool isTrack = true,
      bool ignoreFilters = false,
      Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (include != null)
                query = include(query);

            if (!isTrack)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }



        // ID’ye göre ilgili kaydı getirir (filtresiz olarak da çağrılabilir)
        public async Task<T?> GetByIdAsync(int id, bool ignoreFilters = false)
        {
            if (ignoreFilters)
                return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);

            return await _dbSet.FindAsync(id);
        }

        // Seçim, filtreleme, sıralama ve ilişki dahil etme ile tek bir kayıt döndürür
        public async Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null)
        {
            IQueryable<T> query = _dbSet;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                return await orderBy(query).Select(select).FirstOrDefaultAsync();
            else
                return await query.Select(select).FirstOrDefaultAsync();
        }

        // Seçim, filtreleme, sıralama ve ilişki dahil etme ile birden fazla kayıt döndürür
        public async Task<IEnumerable<TResult>> GetFilteredListAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null)
        {
            IQueryable<T> query = _dbSet;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync();
            else
                return await query.Select(select).ToListAsync();
        }

        // Soft delete (yumuşak silme) – sadece statü değiştirilir
        public void SoftDelete(T item)
        {
            item.SoftDelete(); // IBase arayüzünde tanımlı bir metot olmalı
            _dbSet.Update(item);
        }

        // Bir kaydı günceller
        public void Update(T item)
        {
            _dbSet.Update(item);
        }

        public async  Task<IEnumerable<T>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }
    }

}
